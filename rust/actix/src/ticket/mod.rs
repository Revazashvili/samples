pub mod models;
pub mod handlers;

use actix_web::{web,guard};
use handlers::*;

pub fn config(cfg: &mut web::ServiceConfig) {
    cfg.service(
        web::scope("/api")
        .guard(guard::Header("api-key", "actix"))
        .service(post_ticket)
        .service(get_tickets)
        .service(get_ticket)
        .service(update_ticket)
        .service(delete_ticket));
}