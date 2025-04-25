<template>
  <div class="container">
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
      <button @click="$router.push('/register')" class="reg">Kayıt Ol</button>
    </form>

    <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
  </div>
</template>

<script>
  import axios from 'axios';

  export default {
    data() {
      return {
        username: '',
        password: '',
        errorMessage: '' // Hata mesajını buraya ekleyeceğiz
      };
    },
    methods: {
      async handleSubmit() {
        this.errorMessage = ''; // Önceki hatayı temizleyelim
        try {
          const response = await axios.post('http://localhost:5295/api/user/login', {
            username: this.username,
            password: this.password
          });

          console.log('Login successful', response.data);

          // Kullanıcı ID ve Role bilgisini sakla (localStorage veya Vuex kullanabilirsin)
          const role = response.data.role === 0 ? 'Admin' : 'Employee'; // 0 -> Admin, 1 -> Employee
          const token = response.data.token;
          localStorage.setItem('token', token);  // Token'ı localStorage'a kaydediyoruz
          localStorage.setItem('userId', response.data.id);
          localStorage.setItem('userRole', role);
          axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
          // Kullanıcıyı anket sayfasına yönlendir
          this.$router.push('/survey-list');

        } catch (error) {
          // Hata durumunda gelen mesajı kontrol et
          if (error.response) {
            // Eğer 401 hatası alırsak, kullanıcı adı veya şifre hatalı demek
            const errorMessage = error.response.data?.message || 'Login failed. Please check your credentials.';
            this.errorMessage = errorMessage; // Hata mesajını Vue bileşeninde göster
            console.error('Login failed', errorMessage);
          } else if (error.request) {
            // Sunucuya yapılan istek başarılı ancak yanıt alınamamışsa
            this.errorMessage = 'Server not responding. Please try again later.';
            console.error('Server not responding:', error.request);
          } else {
            // Diğer hata türleri
            this.errorMessage = 'An error occurred. Please try again.';
            console.error('Error', error.message);
          }
        }
      }
    }
  };
</script>

<style scoped>
  .container {
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
    display: block;
    margin-bottom: 5px;
  }

  .reg {
    width: 25%;
    background-color: #04AA6D; /* Green */
    text-align: center;
    text-decoration: none;
  }

  button:hover {
    background-color: #ffd800;
  }

  .error-message {
    color: red;
    text-align: center;
    margin-top: 10px;
  }
</style>
