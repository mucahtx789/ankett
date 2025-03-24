<template>
  <div class="survey-list">
    <h2>Surveys</h2>
    <div v-if="surveys.length > 0">
      <div v-for="survey in surveys" :key="survey.id" class="survey-item">
        <h3>{{ survey.title }}</h3>
        <button @click="takeSurvey(survey.id)">Take Survey</button>
      </div>
    </div>
    <div v-else>
      <p>No surveys available.</p>
    </div>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      surveys: [],
    };
  },
  created() {
    this.getSurveys();
  },
  methods: {
    async getSurveys() {
      try {
        const response = await axios.get('https://your-api-url/api/survey');
        this.surveys = response.data;
      } catch (error) {
        console.error('Failed to fetch surveys', error);
      }
    },
    takeSurvey(surveyId) {
      this.$router.push(`/survey/${surveyId}`);
    },
  },
};
</script>

<style scoped>
  /* Stil ekleyebilirsiniz */
</style>
