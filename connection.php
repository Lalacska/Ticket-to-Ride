<?php

	// Here we give the different global variabels our username, hostname, password and db_name. 
	$host = "localhost"; // Host name 
	$db_username = "user"; // Mysql username 
	$db_password = "password"; // Mysql password 
	$db_name = "TicketToTest"; // Database name  

	// This is a connection string. We use this to connect to the database
	$mysqli_conection = mysqli_connect($host, $db_username, $db_password, $db_name)or die("cannot connect"); 
?>