<?php 
session_start();
require('connection.php');
echo"Forum WORKS!!!             :       ^)";

// Check if the user is logged in
if (!isset($_SESSION['user_id'])) {
  echo "<script>alert('You are not logged in');</script>";
  // Redirect the user to the home page using JavaScript
  echo "<script>window.location.href = 'index.html';</script>";
  // Make sure to exit or die after the redirect to prevent further execution of the script
  exit();
}
?>