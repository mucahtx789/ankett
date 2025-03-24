<template>
  <div class="login-container">
    <h1>Login</h1>
    <form @submit.prevent="handleSubmit">
      <div class="form-group">
        <label for="username">Username:</label>
        <input type="text"
               id="username"
               v-model="username"
               placeholder="Enter your username"
               required />
      </div>

      <div class="form-group">
        <label for="password">Password:</label>
        <input type="password"
               id="password"
               v-model="password"
               placeholder="Enter your password"
               required />
      </div>

      <button type="submit">Login</button>
    </form>
  </div>
</template>

<script>
  export default {
    data() {
      return {
        username: '',
        password: '',
      };
    },
    methods: {
      handleSubmit() {
        // Backend'e veri gönderme işlemi burada yapılacak
        console.log('Username:', this.username);
        console.log('Password:', this.password);

        // API'ye login isteği göndermek isterseniz burada axios'u kullanabilirsiniz
        axios
          .post('/login', {
            username: this.username,
            password: this.password,
          })
          .then(response => {
            console.log('Login successful', response.data);
            // Login başarılıysa, kullanıcıyı başka bir sayfaya yönlendirebilirsiniz
            this.$router.push('/survey-list');
          })
          .catch(error => {
            console.error('Login failed', error);
            alert('Login failed');
          });
      },
    },
  };
</script>

<style scoped>
  .login-container {
    max-width: 400px;
    margin: 50px auto;
    padding: 20px;
    border: 1px solid #ccc;
    border-radius: 8px;
    background-color: #f9f9f9;
  }

  h1 {
    text-align: center;
    margin-bottom: 20px;
  }

  .form-group {
    margin-bottom: 15px;
  }

  label {
    display: block;
    margin-bottom: 5px;
  }

  input {
    width: 100%;
    padding: 8px;
    margin: 5px 0;
    border: 1px solid #ccc;
    border-radius: 4px;
  }

  button {
    width: 100%;
    padding: 10px;
    background-color: #007bff;
    color: white;
    border: none;
    border-radius: 4px;
    cursor: pointer;
  }

    button:hover {
      background-color: #0056b3;
    }
</style>
