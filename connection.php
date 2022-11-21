<?php

	// Here we give the different global variabels our username, hostname, password and db_name. 
    $host = "localhost:3306"; // Host name 
	$db_username = "bekbekbe_6649_cg"; // Mysql username 
	$db_password = "EmilogRasmus1!"; // Mysql password 
	$db_name = "bekbekbe_login_sample_db"; // Database name 

	// This is a connection string. We use this to connect to the database
	$mysqli_conection = mysqli_connect($host, $db_username, $db_password, $db_name)or die("cannot connect"); 
?>