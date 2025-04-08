<template>
  <div class="container">

    <h2 class="title">{{ survey.title }}</h2>
    <p class="date">🗓 Oluşturulma Tarihi: {{ formatDate(survey.createdAt) }}</p>

    <div v-for="(question, index) in survey.questions" :key="question.id" class="question-card">
      <h3>{{ question.text }}</h3>
      <canvas :id="'chart-' + index"></canvas>
    </div>
  </div>

</template>

<script>
  import axios from "axios";
  import Chart from "chart.js/auto";

  export default {
    data() {
      return {
        survey: {
          title: "",
          createdAt: "",
          questions: []
        }
      };
    },
    async created() {
      await this.fetchSurveyDetails();
      this.$nextTick(() => {
        this.renderCharts();
      });
    },
    methods: {
      async fetchSurveyDetails() {
        const surveyId = this.$route.params.id;

        if (!surveyId) {
          console.error("Hata: Anket ID alınamadı.");
          return;
        }

        try {
          const response = await axios.get(`http://localhost:5295/api/survey/details/${surveyId}`);
          this.survey = response.data;
        } catch (error) {
          console.error("Anket verileri alınamadı:", error);
        }
      },
      renderCharts() {
        this.survey.questions.forEach((question, index) => {
          const ctx = document.getElementById(`chart-${index}`);
          if (!ctx) return;

          new Chart(ctx, {
            type: "bar",
            data: {
              labels: question.options.map(option => option.text),
              datasets: [
                {
                  label: "Oy Sayısı",
                  data: question.options.map(option => option.voteCount),
                  backgroundColor: ["#FF6384", "#36A2EB", "#FFCE56", "#4BC0C0"],
                  borderWidth: 1
                }
              ]
            },
            options: {
              responsive: true,
              plugins: {
                legend: { display: false }
              }
            }
          });
        });
      },
      formatDate(dateString) {
        return new Date(dateString).toLocaleDateString("tr-TR");
      }
    }
  };
</script>

<style scoped>
  .container {
    max-width: 800px;
    margin: auto;
    text-align: center;
  }

  .title {
    font-size: 24px;
    font-weight: bold;
  }

  .date {
    font-size: 16px;
    color: #555;
  }

  .question-card {
    background: #f9f9f9;
    padding: 20px;
    margin-top: 20px;
    border-radius: 10px;
    box-shadow: 2px 2px 10px rgba(0, 0, 0, 0.1);
  }
</style>
