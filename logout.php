<?php
// Start the session
session_start();
// Unset all session variables
$_SESSION = array();
// Destroy the session
session_destroy();
// Redirect to the login page or any other page you want after logout
header("Location: index.php");
exit();
?>