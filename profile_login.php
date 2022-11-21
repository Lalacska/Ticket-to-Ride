<?php
    session_start();
    include "connection.php";
	if(isset($_POST["email"]) && isset($_POST["password"])){
		$errors = array();
		
		$email = $_POST["email"];
		$password = $_POST["password"];
		
		//Connect to database
		require dirname(__FILE__) . '/connection.php';
		$_SESSION['username'] = $email;
		if ($stmt = $mysqli_conection->prepare("SELECT username, email, password FROM sc_users WHERE email = ? LIMIT 1")) {
			/* bind parameters for markers */
			$stmt->bind_param('s', $email);
				
			/* execute query */
			if($stmt->execute()){
				
				/* store result */
				$stmt->store_result();

				if($stmt->num_rows > 0){
					/* bind result variables */
					$stmt->bind_result($username_tmp, $email_tmp, $password_hash);

					/* fetch value */
					$stmt->fetch();
					
					if(password_verify ($password, $password_hash)){
					    echo "Success<br>";
					    $script = "<script>
                        window.location = 'https://bekbekbek.com/profile_page.php';</script>";
                        echo $script;
					    return;
					}else{
						$errors[] = "Wrong email or password.";
					}
				}else{
					$errors[] = "Wrong email or password.";
				}
				
				/* close statement */
				$stmt->close();
				
			}else{
				$errors[] = "Something went wrong, please try again.";
			}
		}else{
			$errors[] = "Something went wrong, please try again.";
		}
		
		if(count($errors) > 0){
			echo "<script>alert('$errors[0]')</script>";
		}
	}
?>

<!DOCTYPE html>

<link rel="stylesheet" href="profile_login.css">
<!-- Html for login form-->
<body>
  <div class="container">
    <div class="title">Login</div>
    <div class="content">

        <!-- post method to reach db (db = database) -->
      <form  method="post">

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
            if(isset($_GET["newpwd"])){
                if($_GET["newpwd"] == "passwordupdated"){
                    echo '<p class="signupsuccess">Your password has been reset!</p>';
                }
            }
            ?>
            <a href="/reset-password.php" class="fp">Forgot Password?</a>
          </div>
        <div class="button">
            <input id="button" type="submit" value="Login"><br><br>
            <a href="https://bekbekbek.com/profile_page.php"></a><br><br>
        
        <div class="register">
            <input id="register" type="submit" value="Register" onclick="location.href='https://www.BekBekBek.com/signup.php'"><br><br>
            <a href="signup.php"></a><br><br>
        </div>
        </div>
      </form>
    </div>