<template>
  <div class="container">
    <h2>{{ survey.title }}</h2>

    <div v-for="question in survey.questions" :key="question.id">
      <h3>{{ question.text }}</h3>
      <div v-for="option in question.options" :key="option.id">
        <input type="radio" :name="'question-' + question.id" :value="option.id" v-model="answers[question.id]" />
        <label>{{ option.text }}</label>
      </div>
    </div>

    <button @click="submitAnswers">Cevapları Gönder</button>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      survey: {},
      answers: {},
      userId: localStorage.getItem("userId"),
    };
  },
  async created() {
    const surveyId = this.$route.params.id;
    try {
      const response = await axios.get(`http://localhost:5295/api/survey/${surveyId}`);
      this.survey = response.data;
    } catch (error) {
      console.error("Anket yüklenemedi", error);
    }
  },
  methods: {
    async submitAnswers() {
      const formattedAnswers = Object.keys(this.answers).map((questionId) => ({
        employeeId: this.userId,
        optionId: this.answers[questionId],
      }));

      try {
        await axios.post("http://localhost:5295/api/answer/submit", formattedAnswers);
        alert("Cevaplar başarıyla kaydedildi.");
        this.$router.push("/survey-list");
      } catch (error) {
        console.error("Cevaplar kaydedilirken hata oluştu", error);
      }
    },
  },
};
</script>
