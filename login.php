<?php
    //sesion_start to save the users current session//
    session_start();
    include "connection.php";

    //Check if email and password is empty//
	if(isset($_POST["email"]) && isset($_POST["password"])){
		$errors = array();
		
		$email = $_POST["email"];
		$password = $_POST["password"];
		
		//Connect to database
		require dirname(__FILE__) . '/connection.php';
		
		if ($stmt = $mysqli_conection->prepare("SELECT username, email, password FROM sc_users WHERE email = ? LIMIT 1")) {
			
			/* binder parameters for markers */
			$stmt->bind_param('s', $email);
				
			/* execute query */
			if($stmt->execute()){
				
				/* save result */
				$stmt->store_result();

				if($stmt->num_rows > 0){
					/* bind result */
					$stmt->bind_result($username_tmp, $email_tmp, $password_hash);

					/* fetch to get values */
					$stmt->fetch();
					
          //Check if password matches, if not then error.
					if(password_verify ($password, $password_hash)){
					    echo "Success<br>";
					    $script = "<script>
                        window.location = 'http://www.bekbekbek.com/#/home';</script>";
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

<!-- Css starts here. We can also make this into it's own file-->
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
            <input id="text" type="email" name="email" placeholder="Enter your username">
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
            <a href="https://bekbekbek.com/#home"></a><br><br>

        <div class="register">
            <input id="register" type="submit" value="Register" onclick="location.href='https://www.BekBekBek.com/signup.php'"><br><br>
            <a href="signup.php"></a><br><br>
        </div>
        </div>
      </form>
    </div>