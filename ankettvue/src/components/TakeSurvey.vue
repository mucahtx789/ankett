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
    <button @click="submitAnswers">Submit</button>
  </div>
</template>

<script>
  import axios from "axios";

  export default {
    data() {
      return {
        survey: {},
        answers: {}, // kullanıcın seçtiği cevaplar
      };
    },
    methods: {
      async fetchSurvey() {
        const surveyId = this.$route.params.id;
        console.log("Survey ID:", surveyId); // 

        if (!surveyId || isNaN(surveyId)) {
          console.error("Survey ID not found or invalid:", surveyId);
          alert("Error: Invalid survey ID!");
          this.$router.push('/survey-list'); // 
          return;
        }

        try {
          const response = await axios.get(`http://localhost:5295/api/answer/${surveyId}`);
          this.survey = response.data;
        } catch (error) {
          console.error("Survey could not be loaded", error);
          alert("Error: Survey could not be loaded!");
          this.$router.push('/survey-list'); // 
        }
      },
      async submitAnswers() {
        const userId = localStorage.getItem("userId"); // Giriş yapmış kullanıcının kimliği
        if (!userId) {
          alert("Error: User ID not found, please log in again!");
          return;
        }

        const payload = Object.entries(this.answers).map(([questionId, optionId]) => ({
          questionId: parseInt(questionId),
          optionId,
          employeeId: parseInt(userId), // localStorage'dan gerçek kullanıcı kimliği
        }));

        try {
          await axios.post("http://localhost:5295/api/answer/submit", payload);
          alert("Success: Replies sent successfully!");
          this.$router.push('/survey-list'); // 
        } catch (error) {
          console.error("Error occurred while sending answers", error);
          alert("Error: There was an issue submitting your answers!");
        }
      },
    },
    created() {
      this.fetchSurvey();
    },
  };
</script>
