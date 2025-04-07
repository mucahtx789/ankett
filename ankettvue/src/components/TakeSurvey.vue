<template>
  <div>
    <h1>{{ survey.title }}</h1>
    <div v-for="question in survey.questions" :key="question.id">
      <h3>{{ question.text }}</h3>
      <ul>
        <li v-for="option in question.options" :key="option.id">
          <label>
            <input type="radio" :name="'q' + question.id" :value="option.id" v-model="answers[question.id]" />
            {{ option.text }}
          </label>
        </li>
      </ul>
    </div>
    <button @click="submitAnswers">Gönder</button>
  </div>
</template>

<script>
  import axios from "axios";

  export default {
    data() {
      return {
        survey: {},
        answers: {}, // Kullanıcının seçtiği cevaplar
      };
    },
    methods: {
       async fetchSurvey() {
    const surveyId = this.$route.params.id;
    console.log("Gelen Survey ID:", surveyId); // Debug için

    if (!surveyId || isNaN(surveyId)) {
      console.error("Survey ID bulunamadı veya geçersiz:", surveyId);
      alert("Hata: Geçersiz anket ID'si!");
      this.$router.push('/survey-list'); // Ana sayfaya yönlendir
      return;
    }

    try {
      const response = await axios.get(`http://localhost:5295/api/answer/${surveyId}`);
      this.survey = response.data;
    } catch (error) {
      console.error("Anket yüklenemedi", error);
      alert("Anket yüklenemedi!");
      this.$router.push('/survey-list');
    }
      },
      async submitAnswers() {
        const userId = localStorage.getItem("userId"); // Giriş yapan kullanıcının ID'si
        if (!userId) {
          alert("Kullanıcı kimliği bulunamadı, lütfen tekrar giriş yapın!");
          return;
        }
        const payload = Object.entries(this.answers).map(([questionId, optionId]) => ({
          questionId: parseInt(questionId),
          optionId,
          employeeId: parseInt(userId), // LocalStorage'dan gelen gerçek kullanıcı ID'si
        }));

        try {
          await axios.post("http://localhost:5295/api/answer/submit", payload);
          alert("Cevaplar başarıyla gönderildi!");
          this.$router.push('/survey-list'); // anket sayfasına yönlendir
        } catch (error) {
          console.error("Cevaplar gönderilirken hata oluştu", error);
        }
      },
    },
    created() {
      this.fetchSurvey();
    },
  };
</script>
