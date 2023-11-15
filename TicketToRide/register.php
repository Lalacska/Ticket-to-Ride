<?php
// Check if the required POST parameters are set
if (isset($_POST["email"]) && isset($_POST["username"]) && isset($_POST["password1"]) && isset($_POST["password2"])) {
    $errors = array(); // Initialize an array to store validation errors

    // Set rules for maximum lengths of email, username, and password
    $emailMaxLength = 254;
    $usernameMaxLength = 20;
    $usernameMinLength = 3;
    $passwordMaxLength = 19;
    $passwordMinLength = 5;

    // Retrieve values from the POST method for email, username, and passwords
    $email = strtolower($_POST["email"]);
    $username = $_POST["username"];
    $password1 = $_POST["password1"];
    $password2 = $_POST["password2"];

    // Validate email
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

    // Validate username
    if (strlen($username) > $usernameMaxLength || strlen($username) < $usernameMinLength) {
        $errors[] = "Incorrect username length, must be between " . strval($usernameMinLength) . " and " . strval($usernameMaxLength) . " characters";
    } else {
        if (!ctype_alnum($username)) {
            $errors[] = "Username must be alphanumeric";
        }
    }

    // Validate password
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
                    $errors[] = "Password must contain at least 1 letter and 1 number";
                }
            }
        }
    }

    // Check if there is a user already registered with the same email or username
    if (count($errors) == 0) {
        // Connect to the database
        require dirname(__FILE__) . '/connection.php';

        if ($stmt = $mysqli_connection->prepare("SELECT username, email FROM sc_users WHERE email = ? OR username = ? LIMIT 1")) {
            // Bind parameters for the query
            $stmt->bind_param('ss', $email, $username);

            // Execute the query
            if ($stmt->execute()) {
                // Store the result
                $stmt->store_result();

                if ($stmt->num_rows > 0) {
                    // Bind result variables
                    $stmt->bind_result($username_tmp, $email_tmp);

                    // Fetch the values
                    $stmt->fetch();

                    if ($email_tmp == $email) {
                        $errors[] = "User with this email already exists.";
                    } else if ($username_tmp == $username) {
                        $errors[] = "User with this name already exists.";
                    }
                }

                // Close the statement
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

    // Finalize registration
    if (count($errors) == 0) {
        $hashedPassword = password_hash($password1, PASSWORD_BCRYPT);
        if ($stmt = $mysqli_connection->prepare("INSERT INTO sc_users (username, email, password) VALUES(?, ?, ?)")) {
            // Bind parameters for the query
            $stmt->bind_param('sss', $username, $email, $hashedPassword);

            // Execute the query
            if ($stmt->execute()) {
                // Close the statement
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

    // Check for errors and display the first error, or redirect to login.php
    if (count($errors) > 0) {
        echo $errors[0];
    } else {
        header("location: login.php");
    }
} else {
    echo "Missing data";
}

// Function to validate an email address using a regular expression
function validate_email_address($email)
{
    return preg_match('/^([a-z0-9!#$%&\'*+-\/=?^_`{|}~.]+@[a-z0-9.-]+\.[a-z0-9]+)$/i', $email);
}
?>