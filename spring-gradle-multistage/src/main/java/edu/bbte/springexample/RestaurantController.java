package edu.bbte.springexample;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.client.RestTemplate;

import java.util.Arrays;
import java.util.List;

@RestController
public class RestaurantController {

    private final RestTemplate restTemplate = new RestTemplate();

    // Az URL-eket környezeti változókból olvassuk ki
    private final String server1Url = System.getenv("SERVER1_URL");
    private final String server2Url = System.getenv("SERVER2_URL");

    @GetMapping("/osszes")
    public List<Object> getAllRestaurants() {
        List<Object> server1Restaurants = restTemplate.getForObject(server1Url + "/restaurants", List.class);
        List<Object> server2Restaurants = restTemplate.getForObject(server2Url + "/menus", List.class);

        server1Restaurants.addAll(server2Restaurants); // Összeolvasztás
        return server1Restaurants;
    }

    @GetMapping("/osszes/{id}")
    public Object getRestaurantById(@PathVariable int id) {
        Object server1Restaurant = restTemplate.getForObject(server1Url + "/restaurant/" + id, Object.class);
        Object server2Restaurant = restTemplate.getForObject(server2Url + "/menu/" + id, Object.class);

        return Arrays.asList(server1Restaurant, server2Restaurant);
    }
}
