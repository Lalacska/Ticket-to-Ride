<!DOCTYPE html>
<html lang="en">

<head>
  <title>Registration Page</title>
  <link rel="stylesheet" type="text/css" href="css/bootstrap.css" />
  <link rel="stylesheet" href="styles/register-style.css">
</head>

<body>
  <div class="container">
    <div class="back-link">
      <!--Attachhing id for the javarascript to the link-->
      <a href="#" id="executeLink">Go Home</a>
    </div>
    <div class="title">Registration</div>
    <div class="content">

      <!-- post method to reach db (db = database) -->
      <form action="register.php" method="post">

        <!-- core classes so css can style it. -->
        <div class="user-details">
          <div class="input-box">
            <span class="details">Username</span>
            <input type="text" class="form-control" id="username" name="username" placeholder="Enter your username">
          </div>

          <div class="input-box">
            <span class="details">Email</span>
            <input type="text" class="form-control" id="email" name="email" placeholder="Enter your username">
          </div>

          <div class="input-box">
            <span class="details">Password</span>
            <input type="password1" class="form-control" id="password1" name="password1" placeholder="Enter your password" required autocomplete="off">
          </div>

          <div class="input-box">
            <span class="details">Confirm Password</span>
            <input type="password2" class="form-control" id="password2" name="password2" placeholder="Enter your password" required autocomplete="off">
          </div>

          <div class="button">
            <input id="button" type="submit" value="Register"><br><br>
            <a href="login.php"></a><br><br>
          </div>
        </div>
      </form>
    </div>
  </div>
  </div>
  <!-- Include jQuery library -->
  <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
  <!-- link to our javascript file for the go home button -->
  <script src="javascripts/script.js"></script>
</body>

</html>