<?php
// Replace with your database credentials
$hostname = "localhost";
$username = "root";
$password = "";
$database = "tickettoride";

// Create a MySQLi connection
$mysqli = new mysqli($hostname, $username, $password, $database);

// Check connection
if ($mysqli->connect_error) {
    die("Connection failed: " . $mysqli->connect_error);
}

// Define the API endpoint
if ($_SERVER['REQUEST_METHOD'] === 'GET') {
    if (isset($_GET['endpoint']) && $_GET['endpoint'] === 'get_data') {
        if (isset($_GET['table'])) {
            $table = $_GET['table'];
            $sql = "SELECT * FROM $table";
            $result = $mysqli->query($sql);

            if ($result) {
                $data = $result->fetch_all(MYSQLI_ASSOC);
                echo json_encode($data);
            } else {
                echo json_encode(['error' => 'Query failed']);
            }
        } else {
            echo json_encode(['error' => 'Table not specified']);
        }
    } else {
        echo json_encode(['error' => 'Invalid endpoint']);
    }
}

// ... (close the MySQLi connection)
?>
