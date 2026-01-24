package com.sheltersync.controller;

import com.sheltersync.model.Animal;
import com.sheltersync.repository.AnimalRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@CrossOrigin(origins = "http://localhost:8080")
@RestController
@RequestMapping("/api/animals")
public class AnimalController {

    @Autowired
    AnimalRepository animalRepository;

    @GetMapping
    public ResponseEntity<List<Animal>> getAllAnimals(@RequestParam(required = false) String status) {
        try {
            List<Animal> animals;
            if (status == null)
                animals = animalRepository.findAll();
            else
                animals = animalRepository.findByStatus(status);

            if (animals.isEmpty()) {
                return new ResponseEntity<>(HttpStatus.NO_CONTENT);
            }
            return new ResponseEntity<>(animals, HttpStatus.OK);
        } catch (Exception e) {
            return new ResponseEntity<>(null, HttpStatus.INTERNAL_SERVER_ERROR);
        }
    }

    @GetMapping("/{id}")
    public ResponseEntity<Animal> getAnimalById(@PathVariable("id") long id) {
        Optional<Animal> animalData = animalRepository.findById(id);

        if (animalData.isPresent()) {
            return new ResponseEntity<>(animalData.get(), HttpStatus.OK);
        } else {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
    }

    @PostMapping
    public ResponseEntity<Animal> createAnimal(@RequestBody Animal animal) {
        try {
            Animal _animal = animalRepository.save(new Animal(animal.getName(), animal.getBreed(), animal.getAge(), animal.getStatus(), animal.getArrivalDate(), animal.getImageUrl()));
            return new ResponseEntity<>(_animal, HttpStatus.CREATED);
        } catch (Exception e) {
            return new ResponseEntity<>(null, HttpStatus.INTERNAL_SERVER_ERROR);
        }
    }

    @PutMapping("/{id}")
    public ResponseEntity<Animal> updateAnimal(@PathVariable("id") long id, @RequestBody Animal animal) {
        Optional<Animal> animalData = animalRepository.findById(id);

        if (animalData.isPresent()) {
            Animal _animal = animalData.get();
            _animal.setName(animal.getName());
            _animal.setBreed(animal.getBreed());
            _animal.setAge(animal.getAge());
            _animal.setStatus(animal.getStatus());
            _animal.setArrivalDate(animal.getArrivalDate());
            _animal.setImageUrl(animal.getImageUrl());
            return new ResponseEntity<>(animalRepository.save(_animal), HttpStatus.OK);
        } else {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<HttpStatus> deleteAnimal(@PathVariable("id") long id) {
        try {
            animalRepository.deleteById(id);
            return new ResponseEntity<>(HttpStatus.NO_CONTENT);
        } catch (Exception e) {
            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
        }
    }
}