mod ticket;

use actix_web::{web,HttpServer,App};
use std::sync::Mutex;
use ticket::models::Ticket;
use ticket::config;

#[actix_web::main]
async fn main() -> std::io::Result<()> {
    println!("server starting...");
    let app_state = web::Data::new(AppState{
        tickets: Mutex::new(vec![
            Ticket::new(1,String::from("Jane Doe")),
            Ticket::new(2,String::from("Patrick Star"))
        ])
    });

    HttpServer::new(move || {
        App::new()
            .app_data(app_state.clone())
            .route("/ping", web::get().to(|| async { "pong" }))
            .configure(config)
    })
    .bind(("127.0.0.1", 7878))?
    .run()
    .await
}

struct AppState{
    tickets: Mutex<Vec<Ticket>>,
}