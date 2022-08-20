pub mod models;
pub mod handlers;

use actix_web::web;
use handlers::*;

pub fn config(cfg: &mut web::ServiceConfig) {
    cfg.service(
        web::scope("/api")
        .service(post_ticket)
        .service(get_tickets)
        .service(get_ticket)
        .service(update_ticket)
        .service(delete_ticket));
}