<template>
  <div>
    <h3>Current balance: ${{ calcTime(this.valetSlip.timeIn) }}</h3>
  </div>
</template>

<script>
import PatronService from "@/services/PatronService.js";
import moment from "moment";

export default {
  name: "patron-car-details",
  currentBalance: "",
  props: ["slipId"],
  data() {
    return {
      valetSlip: {},
    };
  },

  created() {
    PatronService.getValetSlip(this.slipId).then((response) => {
      this.valetSlip = response.data;
    });
  },
  methods: {
    calcTime(date) {
      var now = moment(new Date());
      var end = moment(date);
      var duration = moment.duration(now.diff(end));
      var minutes = duration.asMinutes();
      var calc = (minutes / 60) * 5;
      return calc.toFixed(2);
    },
    currentTime() {
      var today = new Date();
      var date =
        today.getFullYear() +
        "-" +
        (today.getMonth() + 1) +
        "-" +
        today.getDate();
      var time =
        today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
      var dateTime = date + " " + time;
      return dateTime;
    },
    calculateHoursParked() {
      var timeIn = new Date(this.valetSlip.timeIn);
      var timeInHours = timeIn.getHours();

      var c = new Date(this.currentTime());
      var cHours = c.getHours();

      return cHours - timeInHours;
    },
    calculateMinutesParked() {
      var timeIn = new Date(this.valetSlip.timeIn);
      var timeInMinutes = timeIn.getMinutes();

      var c = new Date(this.currentTime());
      var cMinutes = c.getMinutes();
      return Math.floor(Math.abs(cMinutes - timeInMinutes));
    },
    calculateCurrentBalance() {
      let hoursCharges = this.calculateHoursParked() * 5;
      let minuteCharges = (this.calculateMinutesParked() / 60) * 5;
      return (hoursCharges + minuteCharges).toFixed(2);
    },
  },
};
</script>

<style>
</style>