use std::{sync::Arc, fs::{File, self}};
use serde_json::from_reader;
use tokio::sync::Mutex;

use crate::models::Customer;

/// Represents an in memory data store of customer data
pub type Db = Arc<Mutex<Vec<Customer>>>;


/// Initializes the data store
/// 
/// Returns a Db type that either contains customers or is empty.
pub fn init_db() -> Db {
    match File::open("./data/customers.json") {
        Ok(file) => Arc::new(Mutex::new(from_reader(file).unwrap())),
        Err(_) => Arc::new(Mutex::new(Vec::new()))
    }
}