<template>
  <div id="app">
    <!-- Üst Kısımda Oturum Açıldıktan Sonra Görünecek Menü -->
    
      <button class="survey-btn" @click="goToSurveyList">Anket Listesi</button>
      <button class="logout-btn" @click="logout">Çıkış Yap</button>
    </div>

    <!-- Sayfa İçeriği -->
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
      // Anket Listesi Sayfasına Git
      goToSurveyList() {
        this.$router.push("/survey-list");
      },

      // Çıkış Yap
      logout() {
        // Kullanıcı bilgilerini localStorage'dan sil
        localStorage.removeItem('userId');
        localStorage.removeItem('userRole');

        // Login sayfasına yönlendir
        this.$router.push('/login');

        // Çıkış sonrası, buton görünümünü güncelle
        this.isLoggedIn = false;
      },

      
    }
  };
</script>

<style>
  body {
    background: url('https://wallpapers.com/images/featured/colorful-abstract-background-rra8u4adw1ubypzl.jpg') repeat;
    font-family: 'Arial', sans-serif;
    margin: 0;
    padding: 0;
    height: 100vh;
  }

  /* Üst Menü (Navbar) */
  .navbar {
    position: fixed;
    top: 20px;
    right: 20px;
    display: flex;
    gap: 20px;
    z-index: 999; /* Navbar'ı diğer içeriklerin üstüne koyar */
  }

  .survey-btn, .logout-btn {
    padding: 10px 20px;
    border-radius: 8px;
    color: white;
    background-color: rgba(0, 0, 0, 0.7);
    border: none;
    font-size: 16px;
    cursor: pointer;
    transition: 0.3s;
  }

    .survey-btn:hover, .logout-btn:hover {
      background-color: rgba(0, 0, 0, 0.85);
    }

  .logout-btn {
    background-color: #e74c3c;
  }

  .survey-btn {
    background-color: #3498db;
  }

  /* Sayfa İçeriği */
  #app {
    padding-top: 80px; /* Navbar'ın üst kısmını boş bırak */
  }

  button:hover {
    background-color: #ffd800;
  }
  .error-message {
    position: fixed;
    top: 10px;
    left: 50%;
    transform: translateX(-50%);
    padding: 10px;
    background-color: red;
    color: white;
    border-radius: 5px;
  }
</style>
