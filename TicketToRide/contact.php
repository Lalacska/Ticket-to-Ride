<style>
  * {
    margin: 0;
    padding: 0;
  }

  body {
    max-height: 802px;
    display: flex;
    justify-content: center;
    align-items: center;
    padding: 10px;
    background: linear-gradient(135deg, #191919, #ff785a);
    width: 100%;
  }


  .contact {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 20px;
    padding-top: 70px;
  }

  a {
    font-size: 19px;
    font-weight: bold;
    color: #ff785a;
    text-decoration: none;
  }

  .back-link {
    margin-top: 10%;
    margin-left: 7%;
  }

  .back-link a:hover {
    color: #ffff;
  }

  p {
    font-size: 19px;
    font-weight: bold;
    color: #ff785a;
  }

  #CT {
    color: #191919;
    border: solid 3px;
    border-radius: 5px;
    font-size: 20px;
    width: 500px;
    min-width: 80%;
  }

  #FN {
    color: #191919;
    border: solid 3px;
    border-radius: 5px;
    font-size: 20px;
    width: 500px
  }

  #EM {
    color: #191919;
    border: solid 3px;
    border-radius: 5px;
    font-size: 20px;
    width: 500px
  }

  #MB {
    color: #191919;
    border: solid 3px;
    border-radius: 5px;
    font-size: 20px;
    width: 500px;
    height: 200px;
  }

  .btn {
    color: #191919;
    border: solid 3px;
    border-radius: 5px;
  }

  #btn {
    font-size: 20px;
    width: 500px;
  }

  p,
  #CT,
  #FN,
  #EM,
  #MB,
  .btn {
    margin-left: 40px;
  }

  input,
  select,
  textarea:focus {
    outline: none;
  }

  ::placeholder {
    color: #19191969;
  }

  #btn:hover {
    background-color: #ff785a;
    transition: 2s;
  }

  section {
    border: solid;
    background-color: black;
    display: inline;
    width: 600px;
    height: 500px;
  }
</style>


<!DOCTYPE html>
<html lang="en">

<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Document</title>
</head>

<body>
  <!--- We use a form with an action and with an http call its possible to use a api from a company called formspree.io-->
  <form action="https://formspree.io/f/xdojlwro" method="POST">
    <div class="contact">
      <section>
        <div class="topic">
          <p>Topic</p>

          <!---We make a drop down menu with 3 different choices-->
          <select name="CT" id="CT">
            <option value="Account">Account</option>
            <option value="Gameplay">Gameplay</option>
            <option value="Other">Other</option>
          </select>
        </div>

        <!--The 3 following classes is information the user have to type for us to help with the problem-->
        <div class="FN">
          <p>Full Name</p>
          <input type="text" name="Name" id="FN" placeholder="Enter Name" required />
        </div>
        <div class="EM">
          <p>Email</p>
          <input id="EM" type="email" name="Email" placeholder="Enter Email" required />
        </div>
        <div class="MB">
          <p>What is your problem</p>
          <textarea id="MB" name="Message" placeholder="Write your problem here" required></textarea>
        </div>
        <div class="btn">
          <button id="btn" type="submit">Send</button>
        </div>
        <div class="back-link">
          <a href="#" id="executeLink">Go Back</a>
        </div>
      </section>
    </div>
  </form>
  <!-- Include jQuery library -->
  <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>

  <script>
    // Use jQuery to handle the click event
    $(document).ready(function() {
      $('#executeLink').on('click', function(e) {
        e.preventDefault(); // Prevent the default behavior of the link

        // Make an AJAX request to check login status
        $.ajax({
          type: 'POST', // You can also use 'GET' depending on your needs
          url: 'check_login.php', // Replace with the actual file path
          success: function(response) {
            console.log(response);
            // Handle the response from the server
            if (response === 'loggedin') {
              window.location.href = 'home.php';
            } else {
              window.location.href = 'index.html';
            }
          }
        });
      });
    });
  </script>
</body>
</html>