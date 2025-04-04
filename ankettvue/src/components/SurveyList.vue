<template>
  <div class="container">
    <h1>Anket Listesi</h1>

    <!-- Admin için anket oluşturma butonu -->
    <div v-if="userRole === 'Admin'">
      <button @click="$router.push('/create-survey')">Yeni Anket Oluştur</button>
    </div>

    <!-- Anketler listeleniyor -->
    <ul>
      <li v-for="survey in surveys" :key="survey.id">
        <div>
          <span><strong>{{ survey.title }}</strong></span> -
          <span>{{ formatDate(survey.createdAt) }}</span> <!-- Anket oluşturulma tarihi -->
          <!-- Admin için anket detaylarına gitme butonu -->
          <button v-if="userRole === 'Admin'" @click="goToSurveyDetail(survey.id)">
            Detayları Gör
          </button>

          <!-- Employee için anketi çözme butonu, yalnızca çözülmemiş anketler için -->
          <button v-if="userRole === 'Employee' && !completedSurveys.includes(survey.id)"
                  @click="takeSurvey(survey.id)">
            Çöz
          </button>

          <!-- Employee için anketi çözüp çözülemediğini kontrol et -->
          <span v-if="userRole === 'Employee' && completedSurveys.includes(survey.id)">
            Çözüldü
          </span>
        </div>
      </li>
    </ul>
  </div>
</template>

<script>
  import axios from 'axios';

  export default {
    data() {
      return {
        surveys: [], // Tüm anketleri tutacak array
        completedSurveys: [], // Çözülen anketlerin ID'leri
        userRole: localStorage.getItem('userRole'), // Kullanıcının rolünü localStorage'dan alıyoruz
        userId: localStorage.getItem('userId'), // Kullanıcının ID'si
      };
    },
    
    async created() {
      // Kullanıcı rolü doğrulandıktan sonra anketleri getir
      try {
        const surveyResponse = await axios.get('http://localhost:5295/api/survey');
        this.surveys = surveyResponse.data;

        // Eğer kullanıcı Employee ise, çözülen anketleri kontrol et
        if (this.userRole === 'Employee') {
          const completedResponse = await axios.get(`http://localhost:5295/api/survey/completed/${this.userId}`);
          this.completedSurveys = completedResponse.data;
        }
      } catch (error) {
        console.error("Anketler alınırken hata oluştu", error);
      }
    },
    methods: {
      // Admin için anket detay sayfasına yönlendirme
      goToSurveyDetail(surveyId) {
        this.$router.push(`/survey-detail/${surveyId}`);
      },

      // Employee için anketi çözme işlemi
      takeSurvey(surveyId) {
        this.$router.push(`/take-survey/${surveyId}`);
      },

      // Tarih formatlama
      formatDate(dateString) {
        const options = { year: 'numeric', month: 'long', day: 'numeric' };
        return new Date(dateString).toLocaleDateString(undefined, options);
      }
    },
  };
</script>
