use std::env;
use actix_web::{HttpServer, App, web};
use actix_web::middleware::Logger;
use env::set_var;

#[actix_web::main]
async fn main() -> std::io::Result<()> {
    set_var("RUST_BACKTRACE", "1");
    set_var("RUST_LOG", "info");
    env_logger::init();

    HttpServer::new(move ||{
        App::new().wrap(Logger::default()).route("/ping",web::get().to(|| async {"pong"}))
        })
        .bind("127.0.0.1:7878")?
        .run()
        .await
}
