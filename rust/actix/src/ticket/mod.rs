use actix_web::{get, post, put, delete, web, HttpRequest, HttpResponse, Responder, ResponseError};
use actix_web::http::header::ContentType;
use actix_web::http::StatusCode;
use actix_web::body::BoxBody;

use serde::{Serialize, Deserialize};
use serde_json::json;

use std::fmt::Display;
use super::AppState;

pub fn config(cfg: &mut web::ServiceConfig) {
    cfg.service(
        web::scope("/api")
        .service(post_ticket)
        .service(get_tickets)
        .service(get_ticket)
        .service(update_ticket)
        .service(delete_ticket));
    cfg.service(
        web::resource("/app")
            .route(web::get().to(|| async { HttpResponse::Ok().body("app") }))
            .route(web::head().to(HttpResponse::MethodNotAllowed)),
    );
}



#[post("/tickets")]
async fn post_ticket(ticket: web::Json<Ticket>,data: web::Data<super::AppState>) -> impl Responder {
    let new_ticket = Ticket::new(ticket.id, String::from(&ticket.author));    
    let mut tickets = data.tickets.lock().unwrap();

    let response = serde_json::to_string(&new_ticket).unwrap();

    tickets.push(new_ticket);
            
    HttpResponse::Created()
        .content_type(ContentType::json()).body(response)
}

#[get("/tickets")]
async fn get_tickets(data: web::Data<AppState>) -> impl Responder{
    let tickets = data.tickets.lock().unwrap();
    web::Json(json!(*tickets))
}

#[get("/tickets/{id}")]
async fn get_ticket(data: web::Data<AppState>, id: web::Path<u32>) -> Result<Ticket,ErrNoId>{
    let tickets = data.tickets.lock().unwrap();
    let ticket_id = *id;

    match tickets.iter().find(|x| x.id == ticket_id)  {
        Some(ticket) => Ok(Ticket{ id: ticket.id,author: String::from(&ticket.author)}),
        None => Err(ErrNoId{id: ticket_id,err: String::from("Ticket not found")})
    }
}

#[put("tickets/{id}")]
async fn update_ticket(data: web::Data<AppState>, id: web::Path<u32>, ticket: web::Json<Ticket>) -> Result<HttpResponse,ErrNoId> {
    let ticket_id = *id;
    let mut tickets = data.tickets.lock().unwrap();

    let new_ticket = Ticket{id: ticket.id,author: String::from(&ticket.author)};

    let position = tickets.iter()
        .position(|x| x.id == ticket_id);
    
    match position{
        Some(id) => {
            let response = serde_json::to_string(&new_ticket).unwrap();
            tickets[id] = new_ticket;
            Ok(HttpResponse::Ok()
            .content_type(ContentType::json()).body(response))
        },
        None => Err(ErrNoId{id: ticket_id,err: String::from("Ticket not found")})
    }
}

#[delete("/tickets/{id}")]
async fn delete_ticket(id:web::Path<u32>,data: web::Data<AppState>) -> Result<Ticket,ErrNoId> {
    let ticket_id = *id;
    let mut tickets = data.tickets.lock().unwrap();

    match tickets.iter().position(|x| x.id == ticket_id) {
        Some(id) => Ok(tickets.remove(id)),
        None => Err(ErrNoId{id: ticket_id,err: String::from("Ticket not found")})
    }
}


#[derive(Serialize, Deserialize,Debug,Clone)]
pub struct Ticket{
  id: u32,
  author: String,
}

impl Ticket{
    pub fn new(id: u32, author: String) -> Self{ Ticket{ id, author } }
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
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result { write!(f, "{:?}", self) }
}