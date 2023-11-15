<?php
if (isset($_POST["reset-request-submit"])) {

    //Creates token and selector variables

    //Creates converts binary to hexadecimals. Proceeds to create 8 random hexadecimal.
    $selecter = bin2hex(random_bytes(8));
    $token = random_bytes(32);

    //The url for the reset site with the selector variable and token variable in the url.
    $url = "create-new-password.php?selector=" . $selecter . "&validator=" . bin2hex($token);

    //token expires
    $expires = date("U") + 1800;

    require 'connection.php';

    $userEmail = $_POST["email"];

    //deletes the Email from pwdResetEmail table.
    $sql = "DELETE FROM pwdReset WHERE pwdResetEmail=?;";
    $stmt = mysqli_stmt_init($mysqli_connection);
    if (!mysqli_stmt_prepare($stmt, $sql)) {
        echo "There was an error!";
        exit();
    } else {
        mysqli_stmt_bind_param($stmt, "s", $userEmail);
        mysqli_stmt_execute($stmt);
    }

    //Inserts variables into pwdReset.
    $sql = "INSERT INTO pwdReset (pwdResetEmail, pwdResetSelector, pwdResetToken,pwdResetExpire) VALUES (?,?,?,?)";
    $stmt = mysqli_stmt_init($mysqli_connection);
    if (!mysqli_stmt_prepare($stmt, $sql)) {
        echo "There was an error!";
        exit();
    } else {

        //Creates password hash. hash turns the data into a short string of letters/numbers.
        $hashedToken = password_hash($token, PASSWORD_DEFAULT);
        mysqli_stmt_bind_param($stmt, "ssss", $userEmail, $selecter, $hashedToken, $expires);
        mysqli_stmt_execute($stmt);
    }

    mysqli_stmt_close($stmt);
    mysqli_close($mysqli_connection);


    // Email for users
    $to = $userEmail;

    $subject = 'Reset your password for Ticket To Ride';

    $message = '<p>We recieved a password reset request. The link to reset your password is down below. If u didnt make this request, you can ignore this email</p>';
    $message .= '<p>Here is your password reset link: </br>';
    $message .= '<a href="' . $url . '">' . $url . '</a></p>';

    $headers = "From: Ticket To Ride <tickettoridetec@gmail.com
    >\r\n";
    $headers .= "Reply-To: Ticket To Ride tickettoridetec@gmail.com
    \r\n";
    $headers .= "Content-type: text/html\r\n";

    mail($to, $subject, $message, $headers);

    header("location: reset-password.php");

} else {
    header("location: /login.php");
}
?>