<?php
//using phpMailer so that we can actually send an email to the user 
//providing them with an actual link
use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\Exception;

// Include the necessary PHPMailer files
require 'PHPMailer/src/Exception.php';
require 'PHPMailer/src/PHPMailer.php';
require 'PHPMailer/src/SMTP.php';

// Create a new instance of PHPMailer
$mail = new PHPMailer(true); // true enables exceptions
$mail->isSMTP();
$mail->Host       = 'smtp.gmail.com';
$mail->SMTPAuth   = true;
$mail->Username   = 'tickettoridetec@gmail.com'; // Your Gmail address
$mail->Password   = 'sdmz vwzn nxkt mato'; // Gmail app-password
$mail->SMTPSecure = PHPMailer::ENCRYPTION_STARTTLS;
$mail->Port       = 587; // Use 465 for SSL

// Check if the form was submitted
if (isset($_POST["reset-request-submit"])) {

    // TOKEN php script starts
    // Generate a random selector and token for password reset
    $selector = bin2hex(random_bytes(8));
    $token = random_bytes(32);

    // Create the URL for password reset
    $url = "http://localhost/tickettoride-web/tickettoride/create-new-password.php?selector=" . $selector . "&validator=" . bin2hex($token);

    // Set the expiration time for the reset link (1800 seconds = 30 minutes)
    $expires = date("U") + 1800;

    // Include the database connection file
    require 'connection.php';

    // Get the user's email from the form
    $userEmail = $_POST["email"];

    // Delete any existing password reset entries for the user
    $sql = "DELETE FROM pwdReset WHERE pwdResetEmail=?;";
    $stmt = mysqli_stmt_init($mysqli_connection);

    if (!mysqli_stmt_prepare($stmt, $sql)) {
        echo "There was an error";
        exit();
    } else {
        mysqli_stmt_bind_param($stmt, "s", $userEmail);
        mysqli_stmt_execute($stmt);
    }

    // Insert the new password reset entry into the database
    $sql = "INSERT INTO pwdReset (pwdResetEmail, pwdResetSelector, pwdResetToken, pwdResetExpires) VALUES (?,?,?,?);";
    if (!mysqli_stmt_prepare($stmt, $sql)) {
        echo "There was an error";
        exit();
    } else {
        $hashedToken = password_hash($token, PASSWORD_DEFAULT);
        mysqli_stmt_bind_param($stmt, "ssss", $userEmail, $selector, $hashedToken, $expires);
        mysqli_stmt_execute($stmt);
    }

    mysqli_stmt_close($stmt);
    // Token php script ends

    // Creating recovery email start
    // Set up the email details
    $to = $userEmail;
    $subject = "Reset your password for TicketToRide";

    // Compose the email message
    $message = '<p> We received a password reset request. The link to reset your password has been included in this mail. If you did not make this request, you can ignore this email</p>';
    $message .= '<p> Here is your password reset link: </br>';
    $message .= '<a href="' . $url . '">' . $url . '</a></p>';

    // Set up PHPMailer to send the email
    $mail->setFrom('tickettoridetec@gmail.com', 'The TTR Team');
    $mail->addAddress($to);
    $mail->Subject = $subject;
    $mail->Body    = $message;
    $mail->isHTML(true);

    // Enable debugging information (set to 2 for detailed debugging)
    $mail->SMTPDebug = 2; // Set this to 2 for debugging information

    try {
        // Attempt to send the email
        $mail->send();
        header("Location: reset-password.php?reset=success");
    } catch (Exception $e) {
        // Display an error message if the email could not be sent
        echo "Message could not be sent. Mailer Error: {$mail->ErrorInfo}";
    }


    // Creating recovery email end

} else {
    // If the form was not submitted, redirect to the index page
    header("Location: index.php");
}
