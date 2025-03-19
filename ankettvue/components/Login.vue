<template>
  <div class="login-container">
    <h2>Login</h2>
    <form @submit.prevent="login">
      <div>
        <label for="username">Username</label>
        <input v-model="username" type="text" id="username" required />
      </div>
      <div>
        <label for="password">Password</label>
        <input v-model="password" type="password" id="password" required />
      </div>
      <button type="submit">Login</button>
    </form>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      username: '',
      password: '',
    };
  },
  methods: {
    async login() {
      try {
        const response = await axios.post('https://your-api-url/api/user/login', {
          username: this.username,
          password: this.password,
        });

        if (response.status === 200) {
          // Login başarılı ise, token veya kullanıcı bilgilerini saklayabilirsiniz.
          localStorage.setItem('userId', response.data.userId);
          localStorage.setItem('role', response.data.role);
          this.$router.push('/survey-list'); // Ana sayfaya yönlendir
        }
      } catch (error) {
        alert('Login failed!');
      }
    },
  },
};
</script>
