<?php

$email = $_POST["email"];

$token = bin2hex(random_bytes(16));

$token_hash = hash("sha256", $token);

$expiry = date("Y-m-d H:i:s", time() + 60 * 30);

$mysqli = require dirname(__FILE__) . '/connection.php';

var_dump($mysqli); // Add this line for debugging

if ($mysqli === false) {
    die('Error loading database connection');
}

$sql = "UPDATE sc_users
            SET reset_token_hash = ?, reset_token_expires_at = ?
            WHERE email = ?";

$stmt = $mysqli->prepare($sql);

if ($stmt === false) {
    die('Error preparing statement: ' . $mysqli->error);
}

$stmt->bind_param("sss", $token_hash, $expiry, $email);

$stmt->execute();
ob_clean();

if ($mysqli->affected_rows){
    
}



