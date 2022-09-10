use serde::{Serialize,Deserialize};

#[derive(Clone,Debug,Serialize,Deserialize)]
pub struct Customer{
    /// A unique identifier for this customer.
    pub guid: String,
    /// First name
    pub first_name: String,
    /// Last name
    pub last_name: String,
    /// Email address
    pub email: String,
    /// Physical Address
    pub address: String,
}