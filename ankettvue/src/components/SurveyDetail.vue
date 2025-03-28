<template>
  <div class="container">
    <h2>{{ survey.title }}</h2>
    <div v-for="question in survey.questions" :key="question.id">
      <h3>{{ question.text }}</h3>
      <ul>
        <li v-for="option in question.options" :key="option.id">{{ option.text }}</li>
      </ul>
    </div>
    <button @click="$router.push('/survey-list')">Geri Dön</button>
  </div>
</template>

<script>
  import axios from "axios";

  export default {
    data() {
      return {
        survey: {},
      };
    },
    async created() {
      const surveyId = this.$route.params.id;
      try {
        const response = await axios.get(`http://localhost:5295/api/survey/${surveyId}`);
        this.survey = response.data;
      } catch (error) {
        console.error("Anket detayları alınırken hata oluştu", error);
      }
    },
  };
</script>
