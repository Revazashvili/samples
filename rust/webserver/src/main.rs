use std::fs;
use std::net::{TcpListener, TcpStream};
use std::io::{Read, Write};

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

    let contents = fs::read_to_string("index.html").unwrap();

    let response = format!(
        "HTTP/1.1 200 OK\r\nContent-Length: {}\r\n\r\n{}",
        contents.len(),
        contents
    );
    stream.write(response.as_bytes()).unwrap();
    stream.flush().unwrap();
}