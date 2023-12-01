<?php
//starting session
session_start();
// require the connection file
require('connection.php');
// Check if the user is logged in
if (!isset($_SESSION['user_id'])) {
  echo "<script>alert('You are not logged in');</script>";
  // Redirect the user to the home page using JavaScript
  echo "<script>window.location.href = 'index.php';</script>";
  // Make sure to exit or die after the redirect to prevent further execution of the script
  exit();
}

?>

<!DOCTYPE html>
<html lang="en">

<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <link rel="stylesheet" href="styles/leaderboard-style.css">
  <title>Leader Board</title>
</head>

<body>

  <header>
    <?php
    // Include the navbar.php script
    require('navbar.php');
    ?>
  </header>

  <footer>
    <?php
    // Include the footer.php script
    require('footer.php');
    ?>
  </footer>

</body>

</html>