<?php
// Start a new session to manage user sessions
session_start();

// Require the connection file which presumably contains the database connection code
require('connection.php');

// Check if the user is not logged in
if (!isset($_SESSION['user_id'])) {
  // Display an alert message and redirect to the login page
  echo "<script>alert('You are not logged in');</script>";
  // takes you to the index page if you are not logged in
  echo "<script>window.location.href = 'index.php';</script>";
} else {
  // Retrieve the user ID from the session
  $user_id = $_SESSION['user_id'];

  // Check if the form is submitted
  if (isset($_POST['submit'])) {
    // Retrieve new user information from the form
    $newUsername = $_POST['new_username'];
    $newEmail = $_POST['new_email'];
    $newPassword1 = $_POST['new_password1'];
    $newPassword2 = $_POST['new_password2'];

    // Initialize an array to store update queries
    $updateQueries = [];

    // Build and add update queries to the array for non-empty fields
    if (!empty($newUsername)) {
      $updateQueries[] = "UPDATE sc_users SET username = '$newUsername' WHERE user_id = $user_id";
    }

    if (!empty($newEmail)) {
      $updateQueries[] = "UPDATE sc_users SET email = '$newEmail' WHERE user_id = $user_id";
    }

    // Check and update the password if provided and matches confirmation
    if (!empty($newPassword1) && $newPassword1 === $newPassword2) {
      // Hash the new password
      $hashedPassword = password_hash($newPassword1, PASSWORD_BCRYPT);
      // Add the password update query to the array
      $updateQueries[] = "UPDATE sc_users SET password = '$hashedPassword' WHERE user_id = $user_id";
    }

    // Execute all update queries
    foreach ($updateQueries as $query) {
      mysqli_query($mysqli_connection, $query);
    }

    // Display a success message after updating
    echo "<script>alert('Success');</script>";
  }

  // Retrieve user information from the database for display in the form
  $strSQL = "SELECT * FROM sc_users WHERE user_id = $user_id";
  $rs = mysqli_query($mysqli_connection, $strSQL);

  // Check if a user with the given user_id exists
  if ($row = mysqli_fetch_array($rs)) {
    $username = $row["username"];
    $email = $row['email'];
    $pass = $row['password'];
    $regdate = $row['registration_date'];
  } else {
    // Display an error message if the user is not found
    echo "User not found.";
  }
}
?>
<!-- Html starts -->
<!DOCTYPE html>
<html lang="en">

<head>
  <title>Profile Page</title>
  <!-- Set the character set and viewport for better rendering on various devices -->
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <!-- Link to an external stylesheet for styling -->
  <link rel="stylesheet" href="styles/profile-page-style.css">
</head>

<body>

  <body>
    <header>
      <!-- Include a navigation bar using PHP -->
      <?php
      // Include the navbar.php script
      require('navbar.php');
      ?>
    </header>

    <div class="container">
      <!-- Title and content sections for the profile page -->
      <div class="title">Profile</div>
      <div class="content">

        <!-- Form for updating user profile information -->
        <form action="profile_page.php" method="post">
          <!-- User details section -->
          <div class="user-details">


            <!-- Input box for the current username (read-only) -->
            <div class="input-box">
              <span class="details">Current Username</span>
              <input type="text" class="form-control" id="username" name="username" readonly placeholder="<?php print "{$username}" ?>">
            </div>
            <!-- Input box for a new username -->

            <div class="input-box">
              <span class="details">New Username</span>
              <input type="text" class="form-control" id="nu" name="new_username" placeholder="New Username">
            </div>
            <!-- Input box for the current email (read-only) -->

            <div class="input-box">
              <span class="details">Current Email</span>
              <input type="text" class="form-control" id="ce" name="email" readonly placeholder="<?php print "{$email}" ?>">
            </div>
            <!-- Input box for a new email -->

            <div class="input-box">
              <span class="details">New Email</span>
              <input type="text" class="form-control" id="nu" name="new_email" placeholder="New Email">
            </div>
            <!-- Input box for a new password -->

            <div class="input-box">
              <span class "details">New Password</span>
              <input type="password" class="form-control" id="nu" name="new_password1" placeholder="New Password">
            </div>
            <!-- Input box to confirm the new password -->

            <div class="input-box">
              <span class="details">Confirm Password</span>
              <input type="password" class="form-control" id="nu" name="new_password2" placeholder="Confirm Password">
            </div>
            <!-- Button to submit the form and update the profile -->

            <div class="button">
              <input type="submit" name="submit" value="Update Profile"><br><br>
            </div>
          </div>
        </form>
      </div>
    </div>
    </div>
    <!-- Include a footer using PHP -->
    <footer>
      <?php
      // Include the footer.php script
      require('footer.php');
      ?>
    </footer>

  </body>

</html>