package com.sheltersync.repository;

import com.sheltersync.model.Animal;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.List;

@Repository
public interface AnimalRepository extends JpaRepository<Animal, Long> {
    List<Animal> findByStatus(String status);
    List<Animal> findByBreedContainingIgnoreCase(String breed);
}