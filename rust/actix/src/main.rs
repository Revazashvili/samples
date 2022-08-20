use actix_web::{get, post, put, delete, web, App, HttpRequest, HttpResponse, HttpServer, Responder, ResponseError, CustomizeResponder};
use actix_web::http::header::ContentType;
use actix_web::http::StatusCode;
use actix_web::body::BoxBody;

use serde::{Serialize, Deserialize};

use std::fmt::Display;
use std::sync::Mutex;

#[actix_web::main]
async fn main() -> std::io::Result<()> {
    println!("server starting...");
    let app_state = web::Data::new(AppState{
        tickets: Mutex::new(vec![
            Ticket::new(1,String::from("Jane Doe")),
            Ticket::new(1,String::from("Patrick Star"))
        ])
    });

    HttpServer::new(move || {
        App::new()
            .app_data(app_state.clone())
            .service(post_ticket)
            .service(get_tickets)
    })
    .bind(("127.0.0.1", 7878))?
    .run()
    .await
}

#[post("/tickets")]
async fn post_ticket(req: HttpRequest,ticket: web::Json<Ticket>,data: web::Data<AppState>) -> impl Responder{
    println!("{:?}",ticket);
    let new_ticket = Ticket::new(ticket.id, String::from(&ticket.author));    
    let mut tickets = data.tickets.lock().unwrap();
    tickets.push(new_ticket.clone());
    new_ticket.respond_to(&req)
}

#[get("/tickets")]
async fn get_tickets(data: web::Data<AppState>) -> impl Responder{
    let tickets = data.tickets.lock().unwrap();
    
    let response = serde_json::to_string(tickets.as_slice()).unwrap();

    HttpResponse::Ok()
        .content_type(ContentType::json())
        .body(response)
}

struct AppState{
    tickets: Mutex<Vec<Ticket>>,
}

#[derive(Serialize, Deserialize,Debug,Clone)]
struct Ticket{
  id: u32,
  author: String,
}

impl Ticket{
    fn new(id: u32, author: String) -> Self{ Ticket{ id, author } }
}

impl Responder for Ticket {
    type Body = BoxBody;

    fn respond_to(self, _req: &HttpRequest) -> HttpResponse<Self::Body> {
        let res_body = serde_json::to_string(&self).unwrap();
        HttpResponse::Ok()
            .content_type(ContentType::json())
            .body(res_body)
    }
}


#[derive(Debug, Serialize)]
struct ErrNoId {
  id: u32,
  err: String,
}

impl ResponseError for ErrNoId {
    fn status_code(&self) -> StatusCode { StatusCode::NOT_FOUND }

    fn error_response(&self) -> HttpResponse<BoxBody> {
        let body = serde_json::to_string(&self).unwrap();
        let response = HttpResponse::new(self.status_code());
        response.set_body(BoxBody::new(body))
    }
}

impl Display for ErrNoId {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "{:?}", self)
    }
}