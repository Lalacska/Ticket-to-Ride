<?php

// Define a custom class named Connect that extends PDO (PHP Data Objects)
class Connect extends PDO {
    // Constructor method for the Connect class
    public function __construct() {
        // Call the parent class's constructor, which connects to the MySQL database
        parent::__construct("mysql:host=localhost;dbname=tickettoride", 'root', '', array(PDO::MYSQL_ATTR_INIT_COMMAND => "SET NAMES utf8"));
        
        // Set error mode to throw exceptions for better error handling
        $this->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        
        // Disable emulated prepared statements to ensure true prepared statements are used
        $this->setAttribute(PDO::ATTR_EMULATE_PREPARES, false);
    }
}

?>