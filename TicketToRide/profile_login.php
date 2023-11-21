<?php
// Start a session to manage user sessions
session_start();

// Include the database connection file
include "connection.php";

// Check if email and password are set in the POST request
if (isset($_POST["email"]) && isset($_POST["password"])) {
  // Array to store error messages
  $errors = array();

  // Retrieve email and password from the POST request
  $email = $_POST["email"];
  $password = $_POST["password"];

  // Connect to the database
  require dirname(__FILE__) . '/connection.php';

  // Set the username in the session
  $_SESSION['username'] = $email;

  // Prepare a SQL statement to select user data based on the provided email
  if ($stmt = $mysqli_connection->prepare("SELECT username, email, password FROM sc_users WHERE email = ? LIMIT 1")) {
    // Bind the email parameter to the prepared statement
    $stmt->bind_param('s', $email);

    // Execute the query
    if ($stmt->execute()) {
      // Store the result
      $stmt->store_result();

      // Check if a user with the given email exists
      if ($stmt->num_rows > 0) {
        // Bind result variables
        $stmt->bind_result($username_tmp, $email_tmp, $password_hash);

        // Fetch the values
        $stmt->fetch();

        // Verify the provided password with the stored password hash
        if (password_verify($password, $password_hash)) {
          // If the passwords match, display success message and redirect to the profile page
          echo "Success<br>";
          $script = "<script>window.location = 'profile_page.php';</script>";
          echo $script;
          return;
        } else {
          // If passwords don't match, set an error message
          $errors[] = "Wrong email or password.";
        }
      } else {
        // If no user found with the given email, set an error message
        $errors[] = "Wrong email or password.";
      }

      // Close the prepared statement
      $stmt->close();
    } else {
      // If execution of the query fails, set an error message
      $errors[] = "Something went wrong, please try again.";
    }
  } else {
    // If preparing the statement fails, set an error message
    $errors[] = "Something went wrong, please try again.";
  }

  // If there are errors, display an alert with the first error message
  if (count($errors) > 0) {
    echo "<script>alert('$errors[0]')</script>";
  }
}
?>
<!-- Html for login form-->
<!DOCTYPE html>
<html lang="en">

<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <link rel="stylesheet" href="styles/profile-login-style.css">
  <title>Login</title>
</head>

<body>
  <div class="container">
    <div class="title">Login</div>
    <div class="content">

      <!-- post method to reach db (db = database) -->
      <form method="post">

        <!-- core classes so css can style it. -->
        <div class="user-details">
          <div class="input-box">
            <span class="details">Email</span>
            <input id="text" type="text" name="email" placeholder="Enter your username">
          </div>
          <div class="input-box">
            <span class="details">Password</span>
            <input id="text" type="password" name="password" placeholder="Enter your password" required autocomplete="off">
            <?php

            //when the users changed password this message will be displayed.
            if (isset($_GET["newpwd"])) {
              if ($_GET["newpwd"] == "passwordupdated") {
                echo '<p class="signupsuccess">Your password has been reset!</p>';
              }
            }
            ?>
            <a href="/reset-password.php" class="fp">Forgot Password?</a>
          </div>
          <div class="button">
            <input id="button" type="submit" value="Login"><br><br>
            <a href="profile_page.php"></a><br><br>

            <div class="register">
              <input id="register" type="submit" value="Register" onclick="location.href='signup.php'"><br><br>
              <a href="signup.php"></a><br><br>
            </div>
          </div>
      </form>
    </div>
</body>

</html>