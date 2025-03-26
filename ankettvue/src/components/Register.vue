<template>
  <div>
    <h2>Register</h2>
    <form @submit.prevent="registerUser">
      <div>
        <label for="username">Username</label>
        <input type="text" id="username" v-model="username" required />
      </div>

      <div>
        <label for="password">Password</label>
        <input type="password" id="password" v-model="password" required />
      </div>

      <div>
        <label for="role">Role</label>
        <select id="role" v-model="role" required>
          <option :value="0">Admin (IK Personeli)</option>
          <option :value="1">Employee (Personel)</option>
        </select>
      </div>

      <button type="submit">Register</button>

      <div v-if="errorMessage" class="error">{{ errorMessage }}</div>
      <div v-if="successMessage" class="success">{{ successMessage }}</div>
    </form>
  </div>
</template>

<script>
  import axios from "axios";

  export default {
    data() {
      return {
        username: '',
        password: '',
        role: 1, // Varsayılan olarak Employee (1)
        errorMessage: '',
        successMessage: ''
      };
    },
    methods: {
      async registerUser() {
        this.errorMessage = "";
        this.successMessage = "";

        try {
          const response = await axios.post("http://localhost:5295/api/user/register", {
            username: this.username,
            password: this.password,
            role: this.role,
          });

          this.successMessage = response.data.message;
        } catch (error) {
          if (error.response && error.response.data) {
            this.errorMessage = error.response.data.message || "Registration failed!";
          } else {
            this.errorMessage = "An error occurred.";
          }
        }
      },
    },
  };
</script>

<style scoped>
  .error {
    color: red;
  }

  .success {
    color: green;
  }
</style>
