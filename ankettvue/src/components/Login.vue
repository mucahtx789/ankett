<template>
  <div class="container">
    <h1>Login</h1>
    <form @submit.prevent="handleSubmit">
      <div class="form-group">
        <label for="username">Username:</label>
        <input type="text" id="username" v-model="username" required />
      </div>

      <div class="form-group">
        <label for="password">Password:</label>
        <input type="password" id="password" v-model="password" required />
      </div>

      <!-- vue3-recaptcha2 -->
      <div class="form-group">
        <vue-recaptcha ref="recaptcha"
                       :sitekey="siteKey"
                       @verify="onCaptchaResolved"
                       @expire="onCaptchaExpired"
                       @fail="onCaptchaError"
                       @error="onCaptchaError" />
      </div>

      <button type="submit" :disabled="!captchaToken">Login</button>
      <button type="button" @click="$router.push('/register')" class="reg">Kayıt Ol</button>
    </form>

    <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
  </div>
</template>

<script>
  import axios from 'axios'; // Axios importu doğru şekilde yapıldı

  export default {
    data() {
      return {
        username: '',
        password: '',
        captchaToken: '',
        errorMessage: '',
        siteKey: '6LcI-i4rAAAAAC31kisMVX4-8yiKEQ3KQV29oW6Q' // siteKey burada
      };
    },
    methods: {
      // reCAPTCHA doğrulama işlemi
      onCaptchaResolved(token) {
        this.captchaToken = token;
      },
      onCaptchaExpired() {
        this.captchaToken = ''; // Token süresi bittiğinde temizlenir
      },
      onCaptchaError() {
        this.errorMessage = 'reCAPTCHA doğrulaması başarısız oldu.'; // Eğer hata oluşursa kullanıcıya hata mesajı verilir
        this.captchaToken = ''; // Token temizlenir
      },
      async handleSubmit() {
        if (!this.captchaToken) {
          this.errorMessage = 'Lütfen reCAPTCHA doğrulamasını tamamlayın.'; // Captcha tamamlanmadıysa hata verir
          return;
        }

        try {
          const response = await axios.post('http://localhost:5295/api/user/login', {
            username: this.username,
            password: this.password,
            RecaptchaToken: this.captchaToken // CAPTCHA token'ı burada gönderilebilir
          });

          const role = response.data.role === 0 ? 'Admin' : 'Employee'; // Role'ü belirle
          const token = response.data.token; // Kullanıcı token'ı
          localStorage.setItem('token', token);  // Token'ı localStorage'a kaydet
          localStorage.setItem('userId', response.data.id); // Kullanıcı ID'sini kaydet
          localStorage.setItem('userRole', role); // Kullanıcı rolünü kaydet

          // Authorization header'ını ayarla
          axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;

          // Anket sayfasına yönlendir
          this.$router.push('/survey-list');

        } catch (error) {
          // Hata durumunda mesaj göster
          this.errorMessage = error.response?.data?.message || 'Login failed';
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
    text-align: center;
    margin-top: 10px;
    color: black;
  }
</style>
