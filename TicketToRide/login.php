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
<!-- Css starts here. We can also make this into it's own file-->
<style>
    /* Reset default margin, padding, and box model for all elements */
  * {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
    font-family: "Poppins", sans-serif;
  }

   /* Styling for the overall body */
  body {
    height: 100vh;
    display: flex;
    justify-content: center;
    align-items: center;
    padding: 10px;
    background: linear-gradient(135deg, #191919, #ff785a);
    width: 100%;
  }
  
  /* Styling for the main container */
  .container {
    max-width: 700px;
    max-height: 400px;
    height: 100%;
    width: 100%;
    background-color: #fff;
    padding: 25px 30px;
    border-radius: 5px;
    box-shadow: 0 5px 10px rgba(0, 0, 0, 0.15);
    background-color: #191919;
  }
  
  /* Styling for the title inside the container */
  .container .title {
    font-size: 25px;
    font-weight: 500;
    position: relative;
    color: #ff785a;
  }

  /* Adding an underline effect to the title */
  .container .title::before {
    content: "";
    position: absolute;
    left: 0;
    bottom: 0;
    height: 3px;
    width: 30px;
    border-radius: 5px;
    background: linear-gradient(135deg, #191919, #ff785a);
  }

  /* Styling for user details section within a form */
  .content form .user-details {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
    margin: 20px 0 12px 0;
    color: #ff785a;
  }

  /* Styling for input boxes within user details */
  form .user-details .input-box {
    margin-bottom: 15px;
    width: calc(100% / 2 - 20px);
  }

  /* Styling for the user input fields */
  .user-details .input-box input {
    height: 45px;
    width: 100%;
    outline: none;
    font-size: 16px;
    border-radius: 5px;
    padding-left: 15px;
    border: 1px solid #ccc;
    border-bottom-width: 2px;
    transition: all 0.3s ease;
  }

  /* Styling for the input fields when focused */
  .user-details .input-box input:focus {
    border-color: #ff785a;
  }

  /* Styling for buttons section within the form */
  form .button {
    display: flexbox;
    margin-top: 40px;
    margin-left: auto;
    height: 50px;
    max-width: 700px;
    width: 90%;
  }

  /* Styling for the button elements */
  form .button input {
    height: 100%;
    width: 90%;
    border-radius: 5px;
    border: none;
    color: #fff;
    font-size: 18px;
    font-weight: 500;
    letter-spacing: 1px;
    cursor: pointer;
    transition: all 0.3s ease;
    background-color: #ff785a;
  }

  /* Styling for the button elements on hover */
  form .button input:hover {
    background-color: #f74017;
  }

  form .register {
    display: flexbox;
    height: 50px;
    max-width: 80%;
    margin-left: 50px;
  }

  /* Styling for the registration section within the form */
  form .register input {
    height: 100%;
    width: 90%;
    border-radius: 5px;
    border: none;
    color: #fff;
    font-size: 18px;
    font-weight: 500;
    letter-spacing: 1px;
    cursor: pointer;
    transition: all 0.3s ease;
    background-color: #ff785a;
  }

  /* Styling for the 'fp' (forgot password) text */
  .input-box .fp {
    font-size: small;
    color: #ff785a;
  }

  /* Media query for smaller screens (max-width: 584px) */
  @media (max-width: 584px) {
    /* Adjusting the maximum width of the container */
    .container {
      max-width: 100%;
    }
    
    /* Adjusting input box width for user details on smaller screens */
    form .user-details .input-box {
      margin-bottom: 15px;
      width: 100%;
    }

    /* Adjusting the margin of the registration button on smaller screens */
    form .register {
      margin-right: 60px;
    }

    /* Adding scroll behavior for user details section on smaller screens */
    .content form .user-details {
      max-height: 300px;
      overflow-y: scroll;
    }

    /* Styling for the scrollbar on WebKit-based browsers */
    .user-details::-webkit-scrollbar {
      width: 5px;
    }
  }
</style>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Document</title>
</head>
<body>

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
            <input id="text" type="email" name="email" placeholder="Enter your username">
          </div>
          <div class="input-box">
            <span class="details">Password</span>
            <input id="text" type="password" name="password" placeholder="Enter your password" required
              autocomplete="off">
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
              <input id="register" type="submit" value="Register"
                onclick="location.href='signup.php'"><br><br>
              <a href="signup.php"></a><br><br>
            </div>
          </div>
      </form>
    </div>
</body>
</html>