use std::net::{TcpListener, TcpStream};
use std::io::{Read};

fn main() -> std::io::Result<()> {
    let listener = TcpListener::bind("127.0.0.1:7878").unwrap();

    for stream in listener.incoming(){
        match stream{
            Ok(stream) => handle_connection(stream),
            Err(error) => eprintln!("connection failed: {}",error)
        }
    }
    println!("Hello, world!");
    Ok(())
}


fn handle_connection(mut stream: TcpStream){
    let mut buffer = [0;1024];

    stream.read(&mut buffer).unwrap();
    println!("Request: {}",String::from_utf8_lossy(&buffer))
}