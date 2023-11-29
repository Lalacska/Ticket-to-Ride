<?php
// Start a session to save the user's current session
session_start();

// Include the "connection.php" file for database connection
include "connection.php";

if (isset($_SESSION['user_id'])) {
  echo "<script>alert('You are already logged in, please log out to log in again');</script>";
  // Redirect the user to the home page using JavaScript
  echo "<script>window.location.href = 'index.php';</script>";
  // Make sure to exit or die after the redirect to prevent further execution of the script
  exit();
}

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
          $script = "<script>window.location = 'index.php';</script>";
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
  <!-- Set the character set and viewport for responsive design -->
  <meta charset="UTF-8">

  <meta name="viewport" content="width=device-width, initial-scale=1.0">

  <!-- Link to external stylesheet for styling -->
  <link rel="stylesheet" href="styles/login-style.css">
  
  <!-- Set the title of the page -->
  <title>Login</title>
</head>

<body>
  <header>
    <?php
    // Include the navbar.php script to include the navigation bar
    require('navbar.php');
    ?>
  </header>

  <div class="container">

    <!-- Title of the login page -->
    <div class="title">Login</div>
    <div class="content">

      <!-- Form for user login using POST method -->
      <form method="post">

        <!-- User details section with input boxes for email and password -->
        <div class="user-details">
          <div class="input-box">

            <!-- Label for email input -->
            <span class="details">Email</span>

            <!-- Input field for email -->
            <input id="text" type="email" name="email" placeholder="Enter your username">
          </div>
          <div class="input-box">

            <!-- Label for password input -->
            <span class="details">Password</span>

            <!-- Input field for password with required attribute and autocomplete off -->
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

            <!-- Link to the reset-password page -->
            <a href="reset-password.php" class="fp">Forgot Password?</a>
          </div>
          <div class="button">
            <!-- Submit button for form -->
            <input id="button" type="submit" value="Login"><br><br>

            <!-- Link to the index page -->
            <a href="index.php"></a><br><br>

            <!-- Register section with input and link to the signup page -->
            <div class="register">
              <input id="register" value="Register" onclick="location.href='signup.php'"><br><br>
              <a href="signup.php"></a><br><br>
            </div>
          </div>
        </div>
      </form>
    </div>
  </div>

  <?php
  // Include the footer.php script to include the footer
  require('footer.php');
  ?>
</body>

</html>