<?php 
session_start();

// Check if the user is logged in
if (!isset($_SESSION['user_id'])) {
  echo "<script>alert('You are not logged in');</script>";
  // Redirect the user to the home page using JavaScript
  echo "<script>window.location.href = 'index.html';</script>";
  // Make sure to exit or die after the redirect to prevent further execution of the script
  exit();
}
?>

<style>
  *{
 margin: 0%;
 padding: 0%;
}
.banner {
    background-image: url("assets/img/bannerimg.c23adc6469c81725.jfif");
    height: 920px;
    background-position: center;
    background-repeat: no-repeat;
    background-size: cover;
    filter: grayscale(60%);
  }

.navbar-nav{
  display: flex;
}
.navbar-nav a{
  font-family: Cambria, Cochin, Georgia, Times, 'Times New Roman', serif;
  font-size: x-large;
  color: grey;
}
  
  .MT {
    color: rgb(0, 0, 0);
    font-size: 50px;
    font-weight: bold;
    top: 30%;
    left: 35%;
    position: absolute;
  }
  .MP {
    color: rgb(0, 0, 0);
    top: 39%;
    left: 35%;
    font-weight: bold;
    position: absolute;
  }
  
  .AG {
    background-color: #191919;
    color: white;
    width: 100%;
    height: 100%;
  }
  .AT {
    color: white;
    font-size: 50px;
    font-weight: bold;
    margin-left: 30px;
    padding-top: 30px;
  }
  
  .AP {
    color: white;
    font-size: 18px;
    font-weight: bold;
    margin: 70px 30px;
  }
  
  .button{
    background-color: #191919;
    padding: 10px;
  }
  
  .button .AB {
    display: block;
    background-color: #ff785a;
    font-size: 19px;
    padding: 25px;
    border-radius: 10px;
    color: white;
    text-decoration: none;
    width: 8%;
    margin-left: 45%;
  }
  
  .GR {
    background-color: #ff785a;
    max-width: 100%;
  }
  
  .RB {
    padding-top: 30px;
    display: grid;
    justify-content: center;
    font-size: 50px;
    font-weight: bold;
    color: white;
  }
  
  .RV {
    display: block;
    margin: 0 auto;
    background-color: #777;
  }
  
  .RT {
    margin-top: 30px;
    display: grid;
    color: white;
    font-size: 18px;
    font-weight: bold;
    justify-content: center;
  }
  
  .DRB {
    display: grid;
    justify-content: center;
    text-decoration: none;
  }
  
  @media only screen and (max-width:1850px){
    .MT {
      color: rgb(0, 0, 0);
      font-size: 50px;
      font-weight: bold;
      top: 30%;
      left: 35%;
      position: absolute;
    }
    .MP {
      color: rgb(0, 0, 0);
      top: 39%;
      left: 35%;
      font-weight: bold;
      position: absolute;
    }
  
    .AG {
      background-color: #191919;
      color: white;
      width: 100%;
      height: 100%;
    }
  
    .AT {
      font-size: 35px;
      font-weight: bold;
      margin-left: 120px;
    }
    .AP {
      font-size: 17px;
      width: 100%;
      height: 55%;
    }
  
    .AB {
      background-color: #ff785a;
      font-size: 19px;
      padding: 25px;
      border-radius: 10px;
      color: white;
      text-decoration: none;
      width: 8%;
      margin-left: 45%;
    }
  
    .GR {
      background-color: #ff785a;
      max-width: 100%;
    }
  
    .RB {
      padding-top: 30px;
      display: grid;
      justify-content: center;
      font-size: 50px;
      font-weight: bold;
      color: white;
    }
  
    .RV {
      display: block;
      margin: 0 auto;
      background-color: #777;
    }
  
    .RT {
      margin-top: 30px;
      display: grid;
      color: white;
      font-size: 18px;
      font-weight: bold;
      justify-content: center;
    }
  
    .DRB {
      display: grid;
      justify-content: center;
      text-decoration: none;
    }
  }
  
  @media only screen and (max-width:850px) {
  
    .MT {
      color: rgb(0, 0, 0);
      font-size: 50px;
      font-weight: bold;
      top: 15%;
      left: 35%;
      position: absolute;
    }
    .MP {
      color: rgb(0, 0, 0);
      top: 39%;
      left: 35%;
      font-weight: bold;
      position: absolute;
    }
  
    .AG {
      background-color: #191919;
      color: white;
      width: 100%;
      height: 100%;
    }
  
    .AT {
      font-size: 20px;
      font-weight: bold;
      margin-left: 120px;
    }
    .AP {
      font-size: 11px;
      width: 82%;
      height: 60%;
    }
  
    .button{
      width: 100%;
    }
  
    .button .AB {
      display: block;
      background-color: #ff785a;
      font-size: 19px;
      padding: 25px;
      border-radius: 10px;
      color: white;
      text-decoration: none;
      width: 25%;
      margin-left: 160px;
    }
  
    .GR {
      background-color: #ff785a;
      max-width: 100%;
    }
  
    .RB {
      padding-top: 30px;
      display: grid;
      justify-content: center;
      font-size: 50px;
      font-weight: bold;
      color: white;
    }
  
    .RV {
      display: block;
      margin: 0 auto;
      background-color: #777;
    }
  
    .RT {
      margin-top: 30px;
      display: grid;
      color: white;
      font-size: 18px;
      font-weight: bold;
      justify-content: center;
    }
  
    .DRB {
      display: grid;
      justify-content: center;
      text-decoration: none;
    }
  
  }


/*Navbar css*/

.active-link {
    font-weight: bold;
  }
  
  .navbar-nav .nav-link.active-link {
    color: var(--secondary-color);
    text-decoration: underline;
  }
  

  /*Footer css*/

  .footer {
    overflow: hidden;
    position: sticky;
    top: 100%;
    background-color: #000;
    padding: 20px 10px;
  }
  
  h1 {
    color: #fff;
    font-size: 25px;
  }
  
  p {
    color: #fff;
    font-size: 30px;
  }
  
  a {
    display: grid;
    color: #fff;
    text-decoration: none;
    padding: 0px 6px;
    margin-right: 25px;
  }
  
  a:hover {
    color: var(--secondary-color);
  }
    
</style>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Home</title>
</head>
<body>
  <nav class="navbar navbar-expand-lg navbar-light">
    <div class="container-fluid">
      <a class="navbar-brand" routerLink=""></a>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavAltMarkup"
        aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarNavAltMarkup">
        <div class="navbar-nav ms-auto">
          <a class="nav-link" href="Forum/forum.php" >Forum</a>
          <a class="nav-link" href="profile_page.php" >Profile</a>
          <a class="nav-link" href="contact.html" >Contact</a>
          <a href="logout.php">Logout</a>
        </div>
      </div>
    </div>
  </nav>
     
<div class="banner">

</div>

<div class="info">
  <h1 class="MT">Ticket To Ride</h1>
  <p class="MP">From the craggy hillsides of Edinburgh to the sunlit docks of Constantinople, <br> from the dusty
    alleys of
    Pamplona to
    a windswept station in Berlin, <br> Ticket to Ride Europe takes you on an exciting train adventure through <br>
    the
    great
    cities of turn-of-the-century Europe.</p>
</div>


<div class="AG">
  <h1 class="AT">About Ticket To Ride</h1>
  <p class="AP">I need A lot of text XDDD <br>
    Lorem ipsum dolor sit amet, consectetur adipisicing elit. Omnis vero vitae at facere id, odit reiciendis non.
    Cumque
    rem consequatur explicabo eveniet quis, soluta sit iste id nostrum impedit esse?<br>
    Lorem, ipsum dolor sit amet consectetur adipisicing elit. Placeat, deleniti iste repellendus quibusdam nam tenetur
    molestias, aliquid totam doloremque magnam aliquam adipisci corrupti eaque, dolorem dolor? Sit officiis animi
    quam?
    Lorem ipsum dolor sit amet consectetur adipisicing elit. Placeat atque perspiciatis aliquid vel ea quo adipisci
    aliquam quaerat voluptatum ratione excepturi quas iusto nihil fugiat, doloremque molestiae facilis, explicabo
    eveniet. <br><br>
    Lorem ipsum dolor sit amet consectetur adipisicing elit. Vel aliquam nihil nobis id excepturi corrupti iusto.
    Distinctio veritatis architecto labore pariatur vel? Neque quibusdam nesciunt rerum voluptate itaque et corporis.
    Lorem ipsum dolor sit amet consectetur adipisicing elit. Asperiores impedit non exercitationem et, natus id
    reprehenderit enim amet laboriosam qui commodi itaque, porro eos repellendus doloremque, recusandae consequuntur?
    Veritatis, ipsam. <br><br>
    Lorem, ipsum dolor sit amet consectetur adipisicing elit. Natus vero, expedita quam nam blanditiis, alias, magni
    nulla vitae doloribus adipisci dicta maxime. Sed aperiam molestias eos dicta eligendi reprehenderit distinctio.
    Lorem ipsum dolor sit, amet consectetur adipisicing elit. <br><br> Maiores dolore distinctio, quis nobis magnam
    officia
    blanditiis! Corrupti quas illum rerum vel officiis quae nostrum, amet culpa, ullam recusandae quisquam
    ipsum?Lorem,
    ipsum dolor sit amet consectetur adipisicing elit. <br><br> Ad veniam ipsam excepturi pariatur natus explicabo
    optio
    voluptate facere necessitatibus sapiente ea magnam ullam id quo omnis culpa aperiam, suscipit earum!loginlo
    Lorem ipsum dolor sit amet consectetur adipisicing elit. Quia veritatis sint aliquid doloremque, ullam dignissimos
    distinctio molestias cum vitae neque unde corrupti iusto placeat atque. Ratione libero autem accusamus>
  </p>
</div>
  <div class="button">
    <a href="https://www.boardgamecapital.com/ticket-to-ride-rules.htm" class="AB">Learn more</a>
  </div>
<div class="GR">
  <h1 class="RB">Rules</h1>
  <iframe class="RV" width="1220" height="980" src="https://www.youtube.com/embed/yPWqKkMKz3E"
    title="YouTube video player" frameborder="0"
    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
    allowfullscreen></iframe>
  <p class="RT"> We linked to a short youtube video explaining how the rules work. <br>This video can be usefull
    for every
    one such as
    new players, experienced players and returning players</p>
  <a href="https://www.boardgamecapital.com/ticket-to-ride-rules.htm" class="DRB">Download Ticket To Ride Official Rules</a>
</div>

<div class="footer">
    <div class="row">
      <!--We use "col-lg-6" because we are using a display over 1200px. The 6 stands for how many colums bootstrap need to generate-->
      <div class="col-lg-5">
        <p>Ticket To Ride</p>
        <a routerLink="leaderboard">LeaderBoard</a>
        <a href="Forum/forum.php">Forum</a>
        <a href="contact.html">Contact</a>
      </div>
      <div class="col-lg-6">
        <p>Policies</p>
        <a href="https://www.boardgamecapital.com/ticket-to-ride-rules.html">Rules</a>
        <a href="profile_page.php">Profile</a>
        <a href="contact.html">Contact</a>
      </div>
      <div class="col-lg-1">
        <p>Join</p>
        <a href="login.php">Login</a>
        <a href="contact.html">Contact</a>
      </div>
    </div>
  </div>
  
  <script>
    // Output the contents of the $_SESSION array to the console
    console.log(<?php echo json_encode($_SESSION); ?>);
  </script>

</body>
</html>