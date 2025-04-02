<template>
  <div>
    <!-- Çıkış yap butonu sadece kullanıcı giriş yapmışsa görünsün -->
    <button v-if="isLoggedIn" @click="logout">Logout</button>

    <router-view></router-view> <!-- Sayfa içeriği burada render edilir -->
  </div>

</template>

<script>
  export default {
    data() {
      return {
        isLoggedIn: false, // Giriş durumu
      };
    },
    created() {
      this.checkLoginStatus(); // Sayfa ilk yüklendiğinde giriş durumu kontrol edilir
    },
    mounted() {
      // Vue Router'ın geçişlerini dinle
      this.$router.afterEach(() => {
        this.checkLoginStatus(); // Sayfa geçişi sonrası giriş durumu kontrol edilir
      });
    },
    methods: {
      checkLoginStatus() {
        const userId = localStorage.getItem('userId');
        this.isLoggedIn = userId !== null; // userId varsa giriş yapılmış demektir
      },
      logout() {
        // Kullanıcı bilgilerini localStorage'dan sil
        localStorage.removeItem('userId');
        localStorage.removeItem('userRole');

        // Login sayfasına yönlendir
        this.$router.push('/login');

        // Çıkış sonrası, buton görünümünü güncelle
        this.isLoggedIn = false;
      }
    }
  };
</script>

<style scoped>
  button {
    padding: 10px;
    background-color: #007bff;
    color: white;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    position: fixed; /* Sabit konumda kalır */
    top: 10px;
    right: 10px;
  }

    button:hover {
      background-color: #ffd800;
    }
</style>
