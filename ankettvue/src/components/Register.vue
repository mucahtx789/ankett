<template>
  <div class="container">
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
      <button @click="$router.push('/login')">Login</button>

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



    button:hover {
      background-color: #ffd800;
    }
</style>
