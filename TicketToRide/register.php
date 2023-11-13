<?php
if (isset($_POST["email"]) && isset($_POST["username"]) && isset($_POST["password1"]) && isset($_POST["password2"])) {
	$errors = array();

	//Different rules for password and username
	$emailMaxLength = 254;
	$usernameMaxLength = 20;
	$usernameMinLength = 3;
	$passwordMaxLength = 19;
	$passwordMinLength = 5;

	//Post method for our email, username and password
	$email = strtolower($_POST["email"]);
	$username = $_POST["username"];
	$password1 = $_POST["password1"];
	$password2 = $_POST["password2"];

	//Validate email
	if (preg_match('/\s/', $email)) {
		$errors[] = "Email can't have spaces";
	} else {
		if (!validate_email_address($email)) {
			$errors[] = "Invalid email";
		} else {
			if (strlen($email) > $emailMaxLength) {
				$errors[] = 'alert"Email is too long, must be equal or under " . strval($emailMaxLength) . " characters"';
			}
		}
	}

	//Validate username
	if (strlen($username) > $usernameMaxLength || strlen($username) < $usernameMinLength) {
		$errors[] = "Incorrect username length, must be between " . strval($usernameMinLength) . " and " . strval($usernameMaxLength) . " characters";
	} else {
		if (!ctype_alnum($username)) {
			$errors[] = "Username must be alphanumeric";
		}
	}
	//Validate password
	if ($password1 != $password2) {
		$errors[] = "Passwords do not match";
	} else {
		if (preg_match('/\s/', $password1)) {
			$errors[] = "Password can't have spaces";
		} else {
			if (strlen($password1) > $passwordMaxLength || strlen($password1) < $passwordMinLength) {
				$errors[] = "Incorrect password length, must be between " . strval($passwordMinLength) . " and " . strval($passwordMaxLength) . " characters";
			} else {
				if (!preg_match('/[A-Za-z]/', $password1) || !preg_match('/[0-9]/', $password1)) {
					$errors[] = "Password must contain atleast 1 letter and 1 number";
				}
			}
		}
	}
	//Check if there is user already registered with the same email or username
	if (count($errors) == 0) {
		//Connect to database
		require dirname(__FILE__) . '/connection.php';

		if ($stmt = $mysqli_connection->prepare("SELECT username, email FROM sc_users WHERE email = ? OR username = ? LIMIT 1")) {

			/* bind parameters for markers */
			$stmt->bind_param('ss', $email, $username);

			/* execute query */
			if ($stmt->execute()) {

				/* store result */
				$stmt->store_result();

				if ($stmt->num_rows > 0) {

					/* bind result variables */
					$stmt->bind_result($username_tmp, $email_tmp);

					/* fetch value */
					$stmt->fetch();

					if ($email_tmp == $email) {
						$errors[] = "User with this email already exist.";
					} else if ($username_tmp == $username) {
						$errors[] = "User with this name already exist.";
					}
				}

				/* close statement */
				$stmt->close();

				$response = "Success";
                echo $response;

			} else {
				$errors[] = "Something went wrong, please try again.";
			}
		} else {
			$errors[] = "Something went wrong, please try again.";
		}
	}
	//Finalize registration
	if (count($errors) == 0) {
		$hashedPassword = password_hash($password1, PASSWORD_BCRYPT);
		if ($stmt = $mysqli_connection->prepare("INSERT INTO sc_users (username, email, password) VALUES(?, ?, ?)")) {

			/* bind parameters for markers */
			$stmt->bind_param('sss', $username, $email, $hashedPassword);

			/* execute query */
			if ($stmt->execute()) {

				/* close statement */
				$stmt->close();
				$response = "Success";
                echo $response;
			} else {
				$errors[] = "Something went wrong, please try again.";
			}
		} else {
			$errors[] = "Something went wrong, please try again.";
		}
	}
	if (count($errors) > 0) {
		echo $errors[0];
	} else {
		header("location: login.php");
	}
} else {
	echo "Missing data";
}

function validate_email_address($email)
{
	return preg_match('/^([a-z0-9!#$%&\'*+-\/=?^_`{|}~.]+@[a-z0-9.-]+\.[a-z0-9]+)$/i', $email);
}
?>
