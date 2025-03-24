<template>
  <div class="survey-detail">
    <h2>{{ survey.title }}</h2>
    <form @submit.prevent="submitAnswers">
      <div v-for="question in survey.questions" :key="question.id" class="question">
        <h3>{{ question.text }}</h3>
        <div v-for="option in question.options" :key="option.id">
          <input type="radio"
                 :name="question.id"
                 :value="option.id"
                 v-model="answers[question.id]" />
          <label>{{ option.text }}</label>
        </div>
      </div>
      <button type="submit">Submit Answers</button>
    </form>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      survey: null,
      answers: {},
    };
  },
  created() {
    const surveyId = this.$route.params.surveyId;
    this.getSurvey(surveyId);
  },
  methods: {
    async getSurvey(surveyId) {
      try {
        const response = await axios.get(`https://your-api-url/api/survey/${surveyId}`);
        this.survey = response.data;
      } catch (error) {
        console.error('Failed to fetch survey details', error);
      }
    },
    async submitAnswers() {
      const employeeId = localStorage.getItem('userId');
      const answerDtos = Object.keys(this.answers).map((questionId) => {
        return {
          employeeId: parseInt(employeeId),
          optionId: this.answers[questionId],
        };
      });

      try {
        await axios.post('https://your-api-url/api/answer', answerDtos);
        alert('Your answers have been submitted.');
        this.$router.push('/survey-list');
      } catch (error) {
        console.error('Failed to submit answers', error);
        alert('An error occurred while submitting answers.');
      }
    },
  },
};
</script>

<style scoped>
  /* Stil ekleyebilirsiniz */
</style>
