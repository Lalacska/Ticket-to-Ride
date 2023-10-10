<?php
    //sesion_start to save the users current session//
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
                        window.location = 'https://double-suicide.rehab/#/home';</script>";
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