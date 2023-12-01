<?php
	// Here we give the different global variabels our username, hostname, password and db_name. 
	$host = "localhost"; // Host name 
	$db_username = "root"; // Mysql username 
	$db_password = ""; // Mysql password 
	$db_name = "tickettoride"; // Database name  

	// This is a connection string. We use this to connect to the database
	$mysqli_connection = mysqli_connect($host, $db_username, $db_password, $db_name)or die("cannot connect"); 

	if (!$mysqli_connection) {
		die("Connection failed: " . $mysqli_connect_error);
	}
?>