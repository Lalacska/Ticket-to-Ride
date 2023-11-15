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


<style>
	* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
  font-family: "Poppins", sans-serif;
}
body {
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 10px;
  background: linear-gradient(135deg, #191919, #ff785a);
  width: 100%;
}
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
.container .title {
  font-size: 25px;
  font-weight: 500;
  position: relative;
  color: #ff785a;
}
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
.content form .user-details {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  margin: 20px 0 12px 0;
  color: #ff785a;
}

form .user-details .input-box {
  margin-bottom: 15px;
  width: calc(100% / 2 - 20px);
}

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
.user-details .input-box input:focus{
  border-color: #ff785a;
}

form .button {
  display: flexbox;
  margin-top: 40px;
  margin-left: auto;
  height: 50px;
  max-width: 700px;
  width: 90%;
}
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
form .button input:hover {
 background-color: #f74017;
}

form .register {
  display: flexbox;
  height: 50px;
  max-width: 80%;
  margin-left: 50px;
}
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

.input-box .fp{
    font-size: small;
    color: #ff785a;
}

@media (max-width: 584px) {
  .container {
    max-width: 100%;
  }
  form .user-details .input-box {
    margin-bottom: 15px;
    width: 100%;
  }

  form .register {
    margin-right: 60px;
  }
  .content form .user-details {
    max-height: 300px;
    overflow-y: scroll;
  }
  .user-details::-webkit-scrollbar {
    width: 5px;
  }
}

</style>

<!DOCTYPE html>

<link rel="stylesheet" href="profile_login.css">
<!-- Html for login form-->

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
						<input id="text" type="password" name="password" placeholder="Enter your password" required
							autocomplete="off">
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
							<input id="register" type="submit" value="Register"
								onclick="location.href='signup.php'"><br><br>
							<a href="signup.php"></a><br><br>
						</div>
					</div>
			</form>
		</div>