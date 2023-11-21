<?php
// Start a session to save the user's current session
session_start();

// Include the "connection.php" file for database connection
include "connection.php";

// Check if email and password were submitted via POST
if (isset($_POST["email"]) && isset($_POST["password"])) {
  $errors = array();

  // Get email and password from the POST data
  $email = $_POST["email"];
  $password = $_POST["password"];

  // Include the "connection.php" file again (duplicate include, not needed)
  require dirname(__FILE__) . '/connection.php';

  // Prepare a database query to retrieve user data based on email
  if ($stmt = $mysqli_connection->prepare("SELECT user_id, username, email, password FROM sc_users WHERE email = ? LIMIT 1")) {

    // Bind the email parameter to the query
    $stmt->bind_param('s', $email);

    // Execute the query
    if ($stmt->execute()) {

      // Store the query result
      $stmt->store_result();

      if ($stmt->num_rows > 0) {
        // Bind the result columns to variables
        $stmt->bind_result($user_id, $username_tmp, $email_tmp, $password_hash);

        // Fetch the result
        $stmt->fetch();

        // Check if the provided password matches the hashed password
        if (password_verify($password, $password_hash)) {
          // Set session variables for the user
          $_SESSION['user_id'] = $user_id;
          $_SESSION['username'] = $username_tmp;
          $_SESSION['email'] = $email_tmp;

          // Prepare user data for response
          $userData = "$username_tmp | $email_tmp |";
          $response = "Success|" . $userData;
          echo $response;

          // Redirect the user to the home page using JavaScript
          $script = "<script>window.location = 'home.php';</script>";
          echo $script;
          return;
        } else {
          $errors[] = "Wrong email or password.";
        }
      } else {
        $errors[] = "Wrong email or password.";
      }

      // Close the prepared statement
      $stmt->close();
    } else {
      $errors[] = "Something went wrong, please try again.";
    }
  } else {
    $errors[] = "Something went wrong, please try again.";
  }

  // If there are errors, display them using JavaScript
  if (count($errors) > 0) {
    echo "<script>alert('$errors[0]')</script>";
  }
}
?>

<!DOCTYPE html>

<html lang="en">

<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <link rel="stylesheet" href="styles/login-style.css">
  <title>Login</title>
</head>

<body>

  <body>
    <div class="container">
      <div class="back-link">
        <!--Attachhing id for the javarascript to the link-->
        <a href="#" id="executeLink">Go Home</a>
      </div>
      <div class="title">Login</div>
      <div class="content">

        <!-- post method to reach db (db = database) -->
        <form method="post">

          <!-- core classes so css can style it. -->
          <div class="user-details">
            <div class="input-box">
              <span class="details">Email</span>
              <input id="text" type="email" name="email" placeholder="Enter your username">
            </div>
            <div class="input-box">
              <span class="details">Password</span>
              <input id="text" type="password" name="password" placeholder="Enter your password" required autocomplete="off">
              <?php
              // Check if the "newpwd" parameter is set in the URL
              if (isset($_GET["newpwd"])) {
                // Check if the "newpwd" parameter has the value "passwordupdated"
                if ($_GET["newpwd"] == "passwordupdated") {
                  // Display a success message when the password has been successfully reset
                  echo '<p class="signupsuccess">Your password has been reset!</p>';
                }
              }
              ?>
              <a href="reset-password.php" class="fp">Forgot Password?</a>
            </div>
            <div class="button">
              <input id="button" type="submit" value="Login"><br><br>
              <a href="home.php"></a><br><br>

              <div class="register">
                <input id="register" value="Register" onclick="location.href='signup.php'"><br><br>
                <a href="signup.php"></a><br><br>
              </div>
            </div>
        </form>
      </div>
      <!-- Include jQuery library -->
      <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
      <!-- link to our javascript file for the go home button -->
      <script src="javascripts/script.js"></script>
  </body>

</html>