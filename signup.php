<!DOCTYPE html>
<html lang="en">

<head>
  <!-- Page title -->
  <title>Registration Page</title>

  <!-- Linking custom CSS files -->
  <link rel="stylesheet" href="styles/register-style.css">
</head>

<body>
  <!-- Header section including navigation bar -->
  <header>
    <?php
    // Include the navbar.php script
    require('navbar.php');
    ?>
  </header>
  <!-- Main content container -->
  <div class="container">
    <!-- Title of the registration page -->
    <div class="title">Registration</div>
    <div class="content">

      <!-- post method to reach db (db = database) -->
      <!-- Form for user registration, using POST method to interact with the server-side script 'register.php' -->
      <form action="register.php" method="post">

        <!-- core classes so css can style it. -->
        <!-- User details section -->
        <div class="user-details">

          <!-- Input box for email -->
          <div class="input-box">
            <span class="details">Username</span>
            <input type="text" class="form-control" id="username" name="username" placeholder="Enter your username">
          </div>

          <!-- Input box for email -->
          <div class="input-box">
            <span class="details">Email</span>
            <input type="text" class="form-control" id="email" name="email" placeholder="Enter your username">
          </div>
          <!-- Input box for password -->
          <div class="input-box">
            <span class="details">Password</span>
            <input type="password" class="form-control" id="password1" name="password1" placeholder="Enter your password" required autocomplete="off">
          </div>
          <!-- Input box for confirming password -->
          <div class="input-box">
            <span class="details">Confirm Password</span>
            <input type="password" class="form-control" id="password2" name="password2" placeholder="Enter your password" required autocomplete="off">
          </div>
          <!-- Registration button and a link to the login page -->
          <div class="button">
            <input id="button" type="submit" value="Register"><br><br>
            <a href="login.php"></a><br><br>
          </div>
        </div>
      </form>
    </div>
  </div>
  </div>
  <!-- Footer section including footer information -->
  <footer>
    <?php
    // Include the footer.php script
    require('footer.php');
    ?>
  </footer>
</body>

</html>